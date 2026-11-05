# 🚀 MiniCRM - Marketing Hub (.NET + WAHA + MySQL)

Este projeto foi desenvolvido em **C# com ASP.NET Core MVC** e utiliza **Docker** para orquestrar os serviços auxiliares (como banco de dados e API do WhatsApp).

- **ASP.NET Core MVC** → Backend e Frontend (W3.CSS)
- **Entity Framework Core** → ORM para o banco de dados
- **WAHA (WhatsApp HTTP API)** → Integração para disparos de WhatsApp
- **MySQL** → Banco de dados principal
- **Docker** → Gerenciamento dos containers de infraestrutura (WAHA e DB)

---

# 📋 Pré-requisitos

Antes de executar o projeto, instale os softwares abaixo.

---

# 1️⃣ Instalar SDK do .NET 8.0

Necessário para compilar e rodar a aplicação C#.

### Download
https://dotnet.microsoft.com/download/dotnet/8.0

### Validar instalação
Abra o terminal e execute:
```bash
dotnet --version
```

---

# 2️⃣ Instalar Docker Desktop

O Docker é responsável por subir a infraestrutura do projeto (WAHA e Banco de Dados) automaticamente.

### Download
https://www.docker.com/products/docker-desktop/

### Validar instalação
Abra o terminal (CMD ou PowerShell) e execute:
```bash
docker --version
docker compose version
```
*(Certifique-se de que o Docker Desktop está aberto e rodando no painel inferior).*

---

# 3️⃣ Instalar Git

O Git é utilizado para clonar o projeto.

### Download
https://git-scm.com/downloads

---

# 4️⃣ Instalar IDE e Ferramentas

### Visual Studio 2022 (Recomendado)
IDE principal para desenvolvimento C# com suporte nativo ao IIS Express e Entity Framework.
- Download: https://visualstudio.microsoft.com/pt-br/vs/

### DBeaver
Para gerenciar e visualizar os dados no banco de dados.
- Download: https://dbeaver.io/download/

---

# 📥 Clonar o Projeto

Abra o terminal e execute:
```bash
git clone [https://github.com/Everton-Felix-Sales/MiniCRM]
```
Entre na pasta do projeto:
```bash
cd MiniCRM
```

---

# 🐳 Subindo a Infraestrutura (WAHA + Banco de Dados)

Com o Docker Desktop aberto, vá até a pasta onde está o arquivo `docker-compose.yml` e execute:
```bash
docker compose up -d
```
Esse comando irá:
✅ Criar os containers do banco de dados e do WAHA
✅ Inicializar os serviços nas portas 3306 e 3000

---

# 🛠️ Configurando e Rodando o MiniCRM

Após subir o Docker, é preciso configurar o banco na aplicação C#.

### 1. Aplicar Migrations (Criar as tabelas)
No terminal, dentro da pasta raiz do projeto (onde está o `.csproj`), execute:
```bash
dotnet ef database update
```

### 2. Rodar a Aplicação
Execute o comando abaixo ou pressione `F5` no Visual Studio 2022:
```bash
dotnet run
```

---

# 🌐 Acessando os Serviços

## MiniCRM (Aplicação)
Após rodar o `dotnet run`, acesse no navegador:
```txt
https://localhost:5001
```
*(A porta pode variar dependendo da configuração do seu `launchSettings.json`)*

## WAHA (Dashboard do WhatsApp)
Acesse para escanear o QR Code:
```txt
http://localhost:3000
```

---

# 🗄️ Configurando Banco no DBeaver

Se estiver usando o MySQL via Docker:

### Criar conexão
Selecione `MySQL`.

### Configuração
- **Host:** `localhost`
- **Porta:** `3306`
- **Usuário:** `root`
- **Senha:** *(A senha definida no seu docker-compose ou appsettings.json)*

---

# 📁 Estrutura do Projeto

```txt
📦 MiniCRM
 ┣ 📂 Controllers      # Controladores (Cliente, Campanha, Login)
 ┣ 📂 Models           # Entidades e Regras de Negócio
 ┣ 📂 Repositories     # Acesso ao Banco de Dados (EF Core)
 ┣ 📂 Services         # Lógica do Sistema e Integração WAHA
 ┣ 📂 Views            # Interfaces em HTML/W3.CSS
 ┣ 📂 wwwroot          # CSS, Imagens e Scripts (mascaras.js)
 ┣ 📜 appsettings.json # Strings de Conexão e Configurações
 ┣ 📜 docker-compose.yml # Infraestrutura (WAHA + DB)
 ┗ 📜 Program.cs       # Configuração de Injeção de Dependência e Sessão
```

---

# ⚠️ Problemas Comuns

## Falha ao iniciar o adaptador de depuração (Socket closed)
- **Motivo:** O Visual Studio falhou ao conectar ao Chrome/Edge.
- **Solução:** No Visual Studio, vá em *Ferramentas > Opções > Depuração > Geral* e **desmarque** a opção "Habilitar depuração de JavaScript para ASP.NET".

## Erro de Migration (The database cannot be accessed)
- **Motivo:** O banco de dados no Docker não subiu ou a string em `appsettings.json` está errada.
- **Solução:** Verifique com `docker ps` se o banco está rodando e valide a `DefaultConnection`.

## Porta já está em uso
- Verifique se os serviços do WAHA (3000) e do App não estão conflitando com aplicações locais.

---

# 📌 Comandos Úteis

### Docker
```bash
docker compose up -d    # Subir infra
docker compose stop     # Parar infra
docker compose logs -f  # Ver logs do WAHA/DB
```

### .NET / C#
```bash
dotnet build            # Compilar o projeto
dotnet run              # Rodar o projeto
dotnet ef migrations add NomeDaMigration # Criar nova migração
dotnet ef database update # Atualizar banco de dados
```

---

# ✅ Projeto iniciado com sucesso!

Se tudo estiver correto:
### App MiniCRM
```txt
https://localhost:5001
```
### WAHA API
```txt
http://localhost:3000
```
Agora o ambiente está pronto para desenvolvimento 🚀
