```
  ______ _             _____            _     _                         _ 
 |  ____| |           |  __ \          | |   | |                       | |
 | |__  | |___  __ _  | |  | | __ _ ___| |__ | |__   ___   __ _ _ __ __| |
 |  __| | / __|/ _` | | |  | |/ _` / __| '_ \| '_ \ / _ \ / _` | '__/ _` |
 | |____| \__ \ (_| | | |__| | (_| \__ \ | | | |_) | (_) | (_| | | | (_| |
 |______|_|___/\__,_| |_____/ \__,_|___/_| |_|_.__/ \___/ \__,_|_|  \__,_|
```                                                                       
## Overview

This project is a starter template for creating an [Elsa Dashboard](https://elsa-workflows.github.io/elsa-core/docs/installation/installing-elsa-dashboard) based project.  Please note the following:

1. This project is based on the setup for a .NET6 minimal API configuration. If you require additional setup for prior versions of .NET or a more detailed implementation of the IHostedServices model, please refer to the [Elsa Dashboard Quickstart](https://elsa-workflows.github.io/elsa-core/docs/quickstarts/quickstarts-aspnetcore-server-dashboard)

2. The Elsa Dashboard is an graphical interface that works with an Elsa Server in order to view & edit Workflow Definitions, monitor Workflow Instances and view the Workflow Registry of workflows on an Elsa Server instance. The default value of `https://localhost:5000` is set in `HostModel` of `_Host.cshtml.cs`.  To provide a different Elsa Server, you can add the following to your `appsettings.json`.

```json
  "Elsa" : {
    "ServerUrl": "https://localhost:5000"
  }

```

Alternatively you can override the configuration path of `Elsa:ServerUrl` in `_Host.cshtml.cs`.

## Dependencies 

Elsa.Designer.Components.Web
                     

## Getting Started &#128640;

Everything you need to support an Elsa Dashboard application is included 

```
dotnet run
```                                                   