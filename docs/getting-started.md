# Getting Started

Engaging with the SHARP-on-MCP initiative and contributing to its development is straightforward. We encourage community contributions via our [community repo](https://github.com/darena-solutions/meldrx-community-mcp). 

To facilitate rapid development and adoption, the following foundational tools and resources are currently available:

## MCP Server Templates

Ready to build your own MCP server with the specifications

-   [.NET](https://github.com/darena-solutions/meldrx-community-mcp/tree/main/dotnet/servers)
-   [TypeScript (Node.js)](https://github.com/darena-solutions/meldrx-community-mcp/tree/main/typescript/servers)
-   Other languages and frameworks to be determined _(TBD)_

Each template provides:

-   MCP handler scaffolding
-   Pluggable FHIR integration points
-   Auth and header-parsing middleware
-   Default Tools

## Default MCP Server Implementations (with Integrated Tools)

We provide ready-to-deploy, open-source MCP server implementations that incorporate a set of default tools for common healthcare scenarios, primarily leveraging FHIR. These projects are actively evolving with the continuous addition of new tools. 

-   [.NET](https://github.com/darena-solutions/meldrx-community-mcp/tree/main/dotnet/default)
-   [Typescript](https://github.com/darena-solutions/meldrx-community-mcp/tree/main/typescript/default)


## Hosted MCP Server Instances for Testing

For immediate testing and experimentation, public-facing instances of SHARP-on-MCP servers are available.

-   .NET - _(URL Coming Soon)_
-   Typescript - _(URL Coming Soon)_

## Testing Harness and Sandbox Environment

To facilitate robust development, a comprehensive testing harness and live sandbox environment have been established. This suite allows developers to validate their MCP server implementations against the SHARP-on-MCP specification and against real-world healthcare data.

*More details on each coming soon*

- MeldRx Standalone: A dedicated environment for testing specific SHARP-on-MCP interactions.
- External EHR Sandbox FHIR Servers: Integration with industry-standard sandbox environments (e.g., Epic sandbox) to simulate realistic clinical data interactions.
- Sample Tools: A collection of example tools for testing various MCP functionalities.
- Postman Collection: A pre-configured Postman collection is provided to simplify API testing and interaction with SHARP-on-MCP servers.

## Video Tutorials and Demonstrations

To further support our community, we are actively developing a series of video tutorials and demonstrations. These resources will provide visual guidance on setting up, configuring, and utilizing SHARP-on-MCP components, showcasing practical applications and common development workflows. Check back soon for updates and links to our video library!