```ts
  ______ _              _____                          
 |  ____| |            / ____|                         
 | |__  | |___  __ _  | (___   ___ _ ____   _____ _ __ 
 |  __| | / __|/ _` |  \___ \ / _ \ '__\ \ / / _ \ '__|
 | |____| \__ \ (_| |  ____) |  __/ |   \ V /  __/ |   
 |______|_|___/\__,_| |_____/ \___|_|    \_/ \___|_|   
                                                                                            
```                     

## Overview

This project is a starter template for creating an [Elsa Server](https://elsa-workflows.github.io/elsa-core/docs/next/installation/installing-server) based project.  Please note the following:

1. This project is based on the setup for a .NET6 minimal API configuration. If you require additional setup for prior versions of .NET or a more detailed implementation of the IHostedServices model, please refer to the [Elsa Server Quickstart](https://elsa-workflows.github.io/elsa-core/docs/next/quickstarts/quickstarts-aspnetcore-server-api-endpoints)

2. The project itself contains a hard-coded `HeartbeatWorkflow` sourced from the [Elsa Server Quickstart](https://elsa-workflows.github.io/elsa-core/docs/next/quickstarts/quickstarts-aspnetcore-server-api-endpoints).  
    - In difference to workflows that are configured in the designer, hard-coded workflows using the builder pattern with `IWorkflowBuilder`, hard-coded workflows do not require persistence for configuration.  The side-effect is that hard-coded workflows are also fixed and cannot be edited in the designer.  
    - Hard coded workflows; however, can be referenced in the designer by other workflows to create sophisticated [Parent/Child workflows](https://elsa-workflows.github.io/elsa-core/docs/next/guides/guides-parent-child-workflows)

3. Elsa supports a variety of persistence providers. As such, this template does not provide one out of the box. Refer to the docs to add [persistence providers](https://elsa-workflows.github.io/elsa-core/docs/next/installation/installing-persistence)

## Dependencies

- Elsa
- Elsa.Activities.Http
    - Exposes HTTP endpoints and activities that can be used by external applications
- Elsa.Activities.Temporal.Quartz
    - Provides Elsa Activities based on the time utility Quartz
- Elsa.Server.API

## Getting Started &#128640;

Everything you need to support an Elsa Server application is included 

```
dotnet run
```

The `HeartbeatWorkflow` is designed to run every 10 seconds.  You can view the instances that have run by issuing the following requests:

```
GET /v1/workflow-instances
```