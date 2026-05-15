# > Proyecto actualmente en desarrollo y evolución continua.


![](./assets/header.png)

**Monitor** es una herramienta de escritorio desarrollada en WPF utilizando el patrón MVVM, orientada a simplificar tareas de monitoreo, acceso y administración básica de dispositivos dentro de una infraestructura local.

La aplicación fue diseñada con foco en:

- Simplicidad de uso
- Configuración flexible
- Monitoreo rápido de conectividad
- Acceso centralizado a recursos internos
- Facilidad de mantenimiento
- Adaptabilidad frente a cambios en la infraestructura

El objetivo principal es brindar una solución liviana y práctica para usuarios técnicos y también para usuarios con menor experiencia, permitiendo administrar accesos y monitoreo de manera visual e intuitiva.

---

# ✨ Características principales

- Monitoreo de conectividad mediante ping
- Integración con accesos VNC
- Escaneo de puertos mediante Nmap
- Descubrimiento de dispositivos en subred
- Interfaz dinámica basada en Grid
- Arquitectura MVVM
- Persistencia local con SQLite
- Configuración flexible y desacoplada
- Componentes visuales dinámicos
- Herramientas orientadas a infraestructura y redes
  
---

# :link: Tecnologías utilizadas

## UI / Frontend
- WPF (.NET)
- XAML
- Data Binding
- Dynamic Grid Layouts

## Arquitectura
- MVVM
- Dependency Injection
- ICommand / RelayCommand

## Persistencia y datos
- SQLite
- XML Processing

## Networking
- Ping Monitoring
- Nmap Integration
- VNC Integration

---

# :link: Funciones

## 🖥️ Menú de Accesos VNC

El módulo principal de la aplicación permite organizar accesos remotos mediante una interfaz dinámica basada en una Grid configurable.

### Características

- Grilla modular configurable
- Organización visual por sectores o categorías
- Componentes dinámicos
- Accesos rápidos mediante VNC
- Monitoreo visual de conectividad

### Casos de uso

- Servidores
- Cámaras
- Equipos de oficina
- Redes
- Infraestructura
- Producción

---

### 📐 Sistema de Guías

La aplicación incorpora un modo de configuración visual denominado:

```text
Mostrar Guías
```

Este sistema permite:

- Visualizar espacios disponibles dentro de la grilla
- Configurar posiciones de componentes
- Agregar elementos mediante menú contextual

Las guías son utilizadas únicamente durante la configuración del panel.

---

### 🔗 Accesos VNC

Cada componente VNC permite generar accesos rápidos hacia dispositivos remotos.

El usuario puede configurar:

- Nombre
- Dirección IP
- Posición dentro de la interfaz

Al ejecutarse:

- La aplicación abre el cliente VNC instalado localmente
- Se envía automáticamente la IP configurada
- Se intenta establecer conexión remota

---

### 📡 Monitoreo de conectividad

Los accesos incorporan monitoreo visual basado en ping.

| Estado | Significado |
|---|---|
| 🟢 Verde | El dispositivo responde conectividad |
| 🔴 Rojo | El dispositivo no responde |

Esto permite visualizar rápidamente el estado general de disponibilidad de los dispositivos.

---

### 🏷️ Componentes de Título

La interfaz permite agregar títulos visuales para organizar distintas secciones dentro de la grilla.

Su función es exclusivamente organizativa y visual.

---

# ⚖️ Menú Balanzas

Módulo orientado al monitoreo de dispositivos de pesaje dentro de la red local.

### Funcionalidades

- Monitoreo mediante ping
- Indicadores visuales de estado
- Detección rápida de desconexiones
- Vista centralizada de dispositivos

> Algunas funcionalidades presentes en entornos de producción fueron retiradas o limitadas en esta versión por motivos de seguridad.

---

# 🌐 Menú Dispositivos

Permite centralizar accesos y monitoreo de distintos equipos de infraestructura.

### Funcionalidades

- Monitoreo de conectividad
- Apertura rápida de interfaces web
- Acceso directo mediante navegador

### Casos de uso

- Impresoras
- Access Points
- Switches
- Teléfonos IP
- Firewalls
- Equipos de red

---

# 🔍 Menú Análisis de Subred

Herramienta destinada al descubrimiento básico de dispositivos dentro de una subred local.

### Funcionamiento

El usuario define una subred objetivo:

```text
192.168.1
```

La aplicación:

- Escanea direcciones IP
- Realiza pruebas de conectividad
- Detecta hosts accesibles

### Información obtenida

- IP detectadas
- Estado de conectividad
- Dispositivos activos

---

# 🚪 Escaneo de Puertos

Integración con Nmap para realizar análisis más avanzados de red.

### Funcionalidades

- Ejecución de instrucciones Nmap
- Procesamiento automático de XML
- Visualización de resultados dentro de la interfaz

### Información procesada

- Dirección MAC
- Hostname
- Sistema operativo detectado
- Puertos abiertos
- Servicios detectados

### Casos de uso

- Diagnóstico de red
- Auditorías básicas
- Verificación de servicios activos
- Detección de puertos expuestos

---

# ⚙️ Menú Configuración

Panel destinado a administrar parámetros dinámicos de la aplicación.

### Configuraciones disponibles

- Ruta de Nmap
- Gateway de red

Esto permite adaptar la herramienta a distintos entornos sin necesidad de recompilar la aplicación.


# :link: Arquitectura

El proyecto fue desarrollado utilizando el patrón MVVM.

## Beneficios

- Separación clara de responsabilidades
- Mejor mantenibilidad
- Mayor escalabilidad
- Código más limpio y reutilizable
- Facilidad para testing y evolución futura

---

# 🧩 Mecánica Compartida de Interfaz

Varios menús de la aplicación comparten una misma mecánica de funcionamiento basada en una interfaz dinámica construida sobre una Grid configurable.

Esta arquitectura permite reutilizar componentes visuales y mantener una experiencia consistente entre distintos menús del sistema.

## 🏗️ Estructura General

La interfaz está compuesta por:

- Una Grid principal configurable
- Múltiples módulos independientes
- Componentes dinámicos
- Capas visuales superpuestas
- Sistema de configuración mediante guías

Cada módulo funciona como una sección independiente dentro de la interfaz principal, permitiendo organizar componentes según distintas necesidades operativas.

## 📐 Sistema de Guías

La aplicación incorpora un modo de configuración visual denominado:

```text
Mostrar Guías
```

Al activarse:

- Se generan botones guía sobre la Grid
- Los espacios disponibles pueden visualizarse y configurarse
- Los componentes pueden agregarse mediante menú contextual
- La interfaz puede reorganizarse dinámicamente

Las guías son utilizadas exclusivamente durante la configuración del panel y no forman parte del modo operativo normal.

## 🧱 Componentes Dinámicos

Los módulos permiten agregar dos tipos principales de componentes visuales dentro de la Grid.

- Botones 
- Títulos 

Cada componente puede posicionarse dinámicamente dentro de la interfaz.

### 🔘 Botones

Los botones representan componentes interactivos cuyo comportamiento depende del menú donde se encuentren ubicados.

Según el módulo, pueden permitir:

- Apertura de conexiones VNC
- Acceso rápido a interfaces web
- Monitoreo visual mediante ping
- Ejecución de acciones relacionadas a dispositivos
- Visualización rápida de estados de conectividad

Los botones incorporan indicadores visuales de estado para representar disponibilidad o respuesta de los dispositivos monitoreados.

| Estado | Significado |
|---|---|
| 🟢 Verde | El dispositivo responde conectividad |
| 🔴 Rojo | El dispositivo no responde |

La funcionalidad específica de cada botón depende del contexto operativo del menú correspondiente.

### 🏷️ Títulos 

Los títulos son componentes utilizados exclusivamente para organización visual dentro de la interfaz.

Su función principal es:

- Separar secciones
- Mejorar legibilidad
- Organizar categorías
- Facilitar navegación visual

Estos componentes no poseen comportamiento operativo ni interacción funcional.

## 🧩 Arquitectura Multicapa

La interfaz utiliza un sistema basado en múltiples capas visuales superpuestas.

Este enfoque permite:

- Separar lógica visual y operativa
- Mostrar elementos de configuración temporalmente
- Superponer componentes dinámicos
- Mantener flexibilidad de organización

## 🖥️ Módulos que utilizan esta mecánica

Actualmente esta arquitectura es utilizada principalmente por:

- Menú de Accesos VNC
- Menú Balanzas
- Menú Dispositivos

Cada módulo adapta la misma base visual a distintos objetivos funcionales.

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

