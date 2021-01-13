# scaled-activities
This repository can be seen as a boilerplate repository when finished. There may be open [issues](https://github.com/Johanbos/scaled-activities/issues), so take a loot at them after forking.

My goal is to run activities in the smallest timespan possible (and also charge your creditcard with the same rate 🚀 be carefull).

## Criteria for my goal:
- Touch 1 million resources / objects
- Each activity is ACID
- Pay per use

## No green, no joy!
![CodeQL](https://github.com/Johanbos/scaled-activities/workflows/CodeQL/badge.svg)
![Test](https://github.com/Johanbos/scaled-activities/workflows/Test/badge.svg)
![Deploy](https://github.com/Johanbos/scaled-activities/workflows/Deploy/badge.svg)

## Links
- [Azure Naming Convention](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/naming-and-tagging)
- [NuGet CSharpGuidelinesAnalyzer](https://github.com/bkoelman/CSharpGuidelinesAnalyzer)
- [NuGet CodeAnalysis.CSharp](https://github.com/dotnet/roslyn/tree/master/src/Analyzers/CSharp/Analyzers)
- [NuGet StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)

No graph? Go [here](https://mermaid-js.github.io/mermaid-live-editor) and use you copy-paste skills.
:::mermaid
sequenceDiagram
    participant C as Callee
    participant O as Orchestrator
    participant Q as Queue
    participant W as Worker(s)
    participant R as Resource(s)
    %% Start activity
    C->>+O: Start
    O->>+Q: Create messages
    Q->>+W: ProcessMessageStart
    W->>+R: ActivityStart
    R-->>-W: ActivityCompleted
    W-->>-Q: ProcessMessageCompleted
    %% View activity
    C->>+R: View
    R-->>-C: ViewCompleted
    %% Clean activity
    C->>+O: Clean
    O->>+Q: Create messages
    Q->>+W: ProcessMessageStart
    W->>+R: ActivityStart
    R-->>-W: ActivityCompleted
    W-->>-Q: ProcessMessageCompleted
:::
