
# FileShare - Peer-2-Peer (P2P) Application

Welcome to the core repository for FileShare P2P application. This project will build upon the previous project which we have worked on seperately. We are scaling the '.tpk' file parser and the chunk valdiation engine into a fully functional, decentralised Peer-to-Peer (P2P) network. 

The application will now be able to read the '.tpk' metadata file, discover peers holding the corresponding file chunks, fetch those chunks over network sockets, and use our existing validation logic to reassemble the final file.

## Developers

* **Senior Developer / Project Manager:** Tyson Thomas
* **Junior Developers:** Lex Li, Mitra Ahmadi, Michael Culverwell, Nesar Kakar. 
* **Quality Assurance/Tester:** Kareem Kashwaa

## Project Milestones and Timelines

There will be a meeting held every Wednesday (5:30pm-9pm) between the Project Manager Tyson Thomas to discuss progress of the project, task completions and subsequent task allocations. Technical issues can be raised to the project manager for advise.

* **23/07/2026:** Project commencement. First meeting.*
* **05/08/2026:** Milestone 1 Check-In
* **19/08/2026:** Milestone 2 Check-In
* **26/08/2026:** Expected Completion & Release.

## Communication

Communication between the developers will occur via email, discord, teams meetings as well as face to face. Issues / bugs can be discussed via these mediums however it is prudent to log it on Github through branching.

## Project Management / Github

The project will be managed in Github repository which will contain the scaffold (skeleton design). 

Repository URL: `https://github.com/lexli201101/Assignment3FileShare.git`

Directory structure is as follows:

* Master
  * FileShare.Web
  * FileShare.Lib
  * FileShare.Test
  * FileShare.Service
  * FileShare.Console

## System Design and Project Codebase

The design pattern nominated is a layered architecture to allow modulatory, reusability and scalibility of the functions. 

### Internal 

1. **FileShare.Web:** Web-based Application Interface which the users will use to control the P2P Downloads.
2. **FileShare.Lib:** Core Engine which will contain the sharing logic, database related connections and networking instructions. 
3. **FileShare.Test:** A dedicated testing project to verify the integrity of the code and quality assurance prior to submission.
4. **FileShare.Service:** The service will run silently in the background to manage active peer socket connections, network traffic and live file states.
5. **FileShare.Console:** Terminal based CLI for command line interaction with the background service.

### External 

The following two external modules will be used.

1. **TideFileDescription** (Meta) - Reading and Parsing of the .tpk file.
2. **TideFile and MerkleTree** (Core) - File validation.

For example, FileShare.Lib should be able to call the external functions from assessment two rather then hardcoding it into the lib. 

## Documentation & Issue Tracking

In order to maintain a clean codebase, we are going to follow two tracking piplines through **In-Code XML Document** and **Git Issue-Driven Development**.

### 1. Document what the Code Does

Utilise XML Comments to explain what the code does. Developers shoud use C# XML comments directly above our methods. The build system automatically compiles these comments into our official HTML documentation artifact via DocFX. 

#### Step-by-Step Instructions:
1. Open a C# File. 
2. Select a Function and just above the signature, type `///` to automatically populate the XML summary tag.
3. Complete the `<summary>`, `<param>`, and `<returns>` fields detailing method behavior, parameter expectations, and output values.

## Github Workflow & Branching Strategy

To maintain codebase stability across the development team, all work must follow an issue-driven branching workflow:

1. **Master Branch:** Reserved exclusively for stable, production-ready code. Direct pushes to the `Master` branch are restricted.
2. **Branch Naming Conventions:** Developers must create dedicated branches off `Master` for every assigned task. Branch names must follow the format `type-description`:
   * `feature` - For new additions or capabilities (e.g., `feature-login-screen`).
   * `bugfix` - For resolving code issues and errors (e.g., `bugfix-socket-timeout`).
   * `task` - For general project tasks or research (e.g., `task-do-research`).
   * `docs` - For updates to documentation or README files (e.g., `docs-update-readme`).
3. **Issue Tracking:** Every pull request must be linked to a logged GitHub Issue (e.g., `Closes #12`).
4. **Pull Requests & Code Review:** Merging into `Master` requires a Pull Request (PR) accompanied by passing unit tests (`FileShare.Test`) and review by QA / Senior Developer.

## Assessment Project Instructions

View the assessment repository: https://github.com/TAFE-tthom/appdev-assignment03-2026S1

## Dependencies & Environment Setup

### Software & Framework Dependencies
* **Core Runtime & SDK:** .NET 10.0 SDK (Cross-platform support across Windows, macOS, and Linux).
* **Networking Modules:** `System.Net` and `System.Net.Sockets` for low-level TCP/UDP peer socket management and network traffic handling.
* **Frontend Web Stack:** React (Node.js & npm environment) for building the `FileShare.Web` user interface.
* **Database & Persistence:** MySQL Server with Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql` / `MySql.Data`) for local metadata, peer registry, and live state tracking.
* **Documentation Generator:** DocFX for building HTML API reference docs from XML comments.

### Cross-Platform Development Environments
The development team operates across heterogeneous workstation setups:
* **Windows Workstations (2x):** VS Code / Visual Studio + .NET 10 SDK + MySQL.
* **macOS Workstation (1x):** VS Code + .NET 10 SDK + Node.js + MySQL.
* **Linux Workstation (1x):** VS Code / Terminal CLI + .NET 10 SDK + MySQL.

*Note: All C# networking code and database connectors must remain OS-agnostic using standard .NET cross-platform APIs.*
