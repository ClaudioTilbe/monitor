# Aún en proceso!! 


![](./assets/header.png)

# :link: Descripción

Monitor es una herramienta de escritorio desarrollada en WPF utilizando el patrón MVVM, orientada a simplificar tareas de monitoreo, acceso y administración básica de dispositivos dentro de una infraestructura local.

La aplicación fue diseñada con foco en:

- Simplicidad de uso
- Configuración flexible
- Monitoreo rápido de conectividad
- Acceso centralizado a recursos internos
- Facilidad de mantenimiento
- Adaptabilidad frente a cambios en la infraestructura

El objetivo principal es brindar una solución liviana y práctica para usuarios técnicos y también para usuarios con menor experiencia, permitiendo administrar accesos y monitoreo de manera visual e intuitiva.

---

# :link: Tecnologías utilizadas

- WPF (.NET)
- Patrón MVVM
- XML Processing
- Nmap Integration
- Ping / Network Monitoring
- VNC Integration

---

# :link: Funciones

## 🖥️ Menú de Accesos VNC

El menú principal de Accesos VNC consiste en una grilla configurable dividida en módulos visuales.

### Estructura de la grilla

La interfaz está compuesta por:

- Una grilla principal configurable
- 10 módulos independientes dentro del mismo menú
- Componentes visuales dinámicos
- Sistema de configuración visual mediante “Guías”

Cada módulo funciona como una sección independiente de la grilla principal, permitiendo organizar accesos por áreas, sectores o categorías.

Ejemplos:

- Servidores
- Cámaras
- Equipos de oficina
- Redes
- Infraestructura
- Producción

---

## Sistema de Guías

La aplicación incluye un modo de configuración llamado:

```text
Mostrar Guías
```

Al activar esta opción:

- Se generan botones guía sobre la grilla
- Los espacios disponibles pueden configurarse visualmente
- Cada espacio permite agregar componentes mediante menú contextual

Las guías son únicamente utilizadas para configuración y organización del panel.

No forman parte del modo operativo normal.

---

## Componentes disponibles

### Acceso VNC

Permite crear accesos rápidos a dispositivos mediante VNC.

#### Funcionamiento

El usuario configura:

- Nombre
- Dirección IP
- Posición en la grilla

Al presionar el botón:

- La aplicación ejecuta el software VNC instalado localmente
- Se envía la dirección IP configurada
- Se intenta establecer conexión remota automáticamente

---

### Monitoreo de conectividad

Cada botón de acceso posee monitoreo visual basado en ping.

#### Estados

| Estado | Significado |
|---|---|
| Verde | El dispositivo responde conectividad |
| Rojo | El dispositivo no responde ping |

Esto permite utilizar el panel como una herramienta rápida de monitoreo visual de disponibilidad.

---

### Título

Componente visual utilizado para organizar la interfaz.

Características:

- Función exclusivamente visual
- Permite separar secciones
- Facilita lectura y organización del panel

Restricción:

- Un título y un botón no pueden compartir el mismo espacio de la grilla

---

# ⚖️ Menú Balanzas

El menú Balanzas está orientado al monitoreo de conectividad de dispositivos de pesaje dentro de la red local.

## Características

- Monitoreo por ping
- Indicadores visuales de estado
- Detección rápida de desconexiones
- Vista centralizada de dispositivos

El funcionamiento general es similar al sistema utilizado en Accesos VNC.

> Nota:
> La versión utilizada en producción posee funcionalidades adicionales relacionadas a integración y operaciones específicas. Algunas de esas funciones fueron retiradas o limitadas en esta versión por motivos de seguridad.

---

# 🌐 Menú Dispositivos

El menú Dispositivos permite centralizar accesos y monitoreo de distintos equipos de infraestructura.

## Funcionalidades

- Monitoreo de conectividad mediante ping
- Apertura rápida de interfaces web
- Acceso directo vía navegador

La aplicación permite abrir automáticamente la dirección IP del dispositivo en el navegador predeterminado del sistema.

---

## Casos de uso

Especialmente útil para:

- Impresoras
- Access Points
- Switches
- Teléfonos IP
- Firewalls
- Equipos de red
- Interfaces administrativas

---

# 🔍 Menú Análisis de Subred

Herramienta destinada a descubrimiento básico de dispositivos dentro de una subred.

## Funcionamiento

El usuario ingresa:

```text
Subred objetivo
```

Ejemplo:

```text
192.168.1
```

La aplicación:

- Realiza pruebas de conectividad
- Escanea direcciones IP de la subred
- Devuelve una lista de hosts accesibles

---

## Información obtenida

- IP detectadas
- Dispositivos con respuesta
- Estado de conectividad

---

# 🚪 Escaneo de Puertos

La aplicación incorpora integración con Nmap para análisis más avanzados de red.

## Integración con Nmap

La herramienta puede:

- Ejecutar instrucciones Nmap
- Obtener resultados en formato XML
- Procesar automáticamente la información obtenida
- Mostrar resultados relevantes dentro de la interfaz

---

## Información procesada

El sistema puede mostrar:

- Instrucción ejecutada en Nmap
- Dirección MAC del dispositivo
- Hostname
- Sistema operativo detectado (si está disponible)
- Puertos abiertos
- Servicios detectados

---

## Ejemplos de uso

- Diagnóstico de red
- Validación de servicios activos
- Verificación de conectividad
- Auditorías básicas
- Detección rápida de puertos expuestos

---

# ⚙️ Menú Configuración

La aplicación dispone de un menú de configuración para administrar rutas y parámetros dinámicos.

## Configuraciones disponibles

Entre las configuraciones principales:

- Ruta de Nmap
- Gateway de red

Esto permite adaptar la herramienta fácilmente a distintos entornos sin necesidad de recompilar la aplicación.

---

# :link: Arquitectura

El proyecto fue desarrollado utilizando el patrón MVVM.

## Beneficios

- Separación clara de responsabilidades
- Mejor mantenibilidad
- Mayor escalabilidad
- Código más limpio y reutilizable
- Facilidad para testing y evolución futura

---

# :link: Finalidad del proyecto

## Objetivos del software

Monitor fue desarrollado como una pequeña herramienta enfocada en:

- Mejorar la comodidad operativa
- Simplificar tareas repetitivas
- Centralizar accesos internos
- Facilitar monitoreo básico
- Reducir tiempos de acceso y validación
- Brindar una interfaz sencilla para usuarios menos experimentados

La aplicación busca ser:

- Liviana
- Sostenible
- Fácil de configurar
- Adaptable a cambios en la infraestructura local
- Rápida de utilizar en entornos operativos reales

## Objetivos personales

A nivel personal, este proyecto representó una oportunidad para profundizar en el desarrollo de aplicaciones de escritorio modernas utilizando tecnologías del ecosistema .NET, con un enfoque orientado tanto al aprendizaje técnico como al diseño de una herramienta funcional para entornos reales.

Uno de los principales objetivos fue trabajar sobre una interfaz visual minimalista, clara y fluida, utilizando XAML como tecnología principal para la construcción de la UI. El proyecto permitió explorar en profundidad el funcionamiento de WPF, comprendiendo conceptos relacionados a renderizado visual, composición de interfaces, estilos, bindings y manejo dinámico de componentes.

También se hizo especial énfasis en la implementación de una arquitectura MVVM lo más desacoplada y limpia posible, buscando mantener una correcta separación de responsabilidades entre vistas, lógica de negocio y manejo de estados. Esto permitió desarrollar una base más mantenible, escalable y sencilla de extender a futuro.

Otro de los desafíos personales del proyecto fue trabajar con elementos dinámicos dentro de una Grid, logrando implementar un sistema visual compuesto por múltiples capas de componentes. Esto me permitió generar una interfaz configurable por el usuario, con componentes que podían agregarse dinámicamente dentro de distintas secciones del panel.

Durante el desarrollo también se incorporaron tecnologías y conceptos nuevos, entre ellos:

- Inyección de dependencias para desacoplar servicios y mejorar la organización general de la aplicación.
- Integración con SQLite como base de datos liviana y embebida para persistencia local de configuraciones y datos.
- Integración con Nmap para ejecutar análisis de red directamente desde la aplicación.
- Procesamiento de resultados XML generados por Nmap para extraer y mostrar información relevante de manera estructurada.

El proyecto también sirvió como práctica para trabajar con:

- Monitoreo de conectividad mediante ping.
- Interacción con procesos externos del sistema operativo.
- Manipulación dinámica de componentes visuales.
- Diseño de herramientas orientadas a infraestructura y redes.
- Organización de proyectos de escritorio bajo una arquitectura mantenible.

En conjunto, el desarrollo de Monitor funcionó tanto como una herramienta de uso práctico como un espacio de aprendizaje técnico para experimentar distintas tecnologías, patrones y enfoques de diseño dentro del ecosistema WPF y .NET.


---


# Seguridad

Algunas funcionalidades utilizadas internamente en ambientes de producción fueron retiradas o limitadas dentro de esta versión por motivos de seguridad.

