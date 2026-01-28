🚀 Bootcamp CLT - API de Productos
Esta es una API robusta de gestión de productos desarrollada con .NET 8, siguiendo los principios de Clean Architecture y diseñada para ser desplegada en un ecosistema de microservicios con Kubernetes.

🏗️ Arquitectura y Tecnologías
El proyecto implementa una arquitectura desacoplada y escalable:

Capa de API: Controladores RESTful documentados con Swagger.
Capa de Application: Implementación de patrones CQRS mediante la librería MediatR.
Capa de Domain: Entidades de negocio y lógica core.
Capa de Infrastructure: Acceso a datos con Entity Framework Core y PostgreSQL.

🛠️ Stack Tecnológico
Backend: .NET 8.0 (C#)
Base de Datos: PostgreSQL
Contenerización: Docker & Docker Compose
Orquestación: Kubernetes (Minikube) & Helm
CI/CD: GitHub Actions (Self-hosted runner)
Observabilidad: Seq & Serilog para logs estructurados.

🚀 Instalación y Despliegue Local
Pre-requisitos
.NET 8 SDK
Docker Desktop
Minikube + Helm

📈 Pipeline de CI/CD
El proyecto cuenta con una integración continua automatizada que realiza:

Build & Test: Validación del código en cada push.
Dockerization: Creación de imagen automática y subida a Docker Hub.
Continuous Deployment: Actualización automática del clúster mediante Helm (CD).

📝 Notas de Evaluación
Logs Estructurados: Implementado con Serilog para facilitar la trazabilidad en Seq.
Manejo de Nulidad: Código limpio con 0 Warnings de tipos nulables.
Persistencia: Configuración automática de esquemas en PostgreSQL.
