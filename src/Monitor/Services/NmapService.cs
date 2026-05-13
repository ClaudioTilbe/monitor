using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monitor.Services
{
    public class NmapService
    {

        private readonly string _rutaNmap;

        public NmapService(string rutaNmap)
        {
            _rutaNmap = rutaNmap;
        }

        // Ejecutar escaneo completo
        public async Task<ResultadoScan> EscanearAsync(string ip)
        {
            string xml = await EjecutarNmapAsync(ip);
            return ProcesarXml(xml);
        }

        // Ejecutar Nmap
        private Task<string> EjecutarNmapAsync(string ip)
        {
            return Task.Run(() =>
            {
                var output = new StringBuilder();

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _rutaNmap,
                    Arguments = $"-oX - {ip}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proceso = new Process())
                {
                    proceso.StartInfo = psi;

                    proceso.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                            output.AppendLine(e.Data);
                    };

                    proceso.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                            output.AppendLine("ERR: " + e.Data);
                    };

                    proceso.Start();

                    proceso.BeginOutputReadLine();
                    proceso.BeginErrorReadLine();

                    proceso.WaitForExit();
                }

                return output.ToString();
            });
        }

        // Parsear XML
        private ResultadoScan ProcesarXml(string xml)
        {
            var resultado = new ResultadoScan();

            var doc = XDocument.Parse(xml);
            var nmaprun = doc.Element("nmaprun");

            resultado.Comando = nmaprun?.Attribute("args")?.Value;

            var host = nmaprun?.Element("host");
            if (host == null)
                return resultado;

            resultado.IP = host.Elements("address")
                .FirstOrDefault(a => a.Attribute("addrtype")?.Value == "ipv4")
                ?.Attribute("addr")?.Value;

            resultado.MAC = host.Elements("address")
                .FirstOrDefault(a => a.Attribute("addrtype")?.Value == "mac")
                ?.Attribute("addr")?.Value;

            resultado.Hostname = host.Element("hostnames")
                ?.Element("hostname")
                ?.Attribute("name")?.Value;

            resultado.SistemaOperativo = host.Element("os")
                ?.Element("osmatch")
                ?.Attribute("name")?.Value;

            var puertos = host.Element("ports")?.Elements("port");

            if (puertos != null)
            {
                foreach (var p in puertos)
                {
                    var estado = p.Element("state")?.Attribute("state")?.Value;

                    if (estado == "open")
                    {
                        resultado.Puertos.Add(new PuertoInfo
                        {
                            Puerto = int.Parse(p.Attribute("portid").Value),
                            Protocolo = p.Attribute("protocol").Value,
                            Servicio = p.Element("service")?.Attribute("name")?.Value
                        });
                    }
                }
            }

            return resultado;
        }

        // Formatear resultado
        public string FormatearResultado(ResultadoScan r)
        {
            var sb = new StringBuilder();

            sb.AppendLine("===== RESULTADO SCAN =====");
            sb.AppendLine("");

            sb.AppendLine($"Comando: {r.Comando}");
            sb.AppendLine("");

            sb.AppendLine($"IP: {r.IP}");
            sb.AppendLine($"MAC: {r.MAC}");
            sb.AppendLine($"Hostname: {r.Hostname}");
            sb.AppendLine($"SO: {r.SistemaOperativo}");
            sb.AppendLine("");

            sb.AppendLine("Puertos abiertos:");

            foreach (var p in r.Puertos)
            {
                sb.AppendLine($"- {p.Puerto}/{p.Protocolo} ({p.Servicio})");
            }

            return sb.ToString();
        }


    }


}
