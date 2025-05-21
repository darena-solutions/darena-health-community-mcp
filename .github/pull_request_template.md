### Description

_Provide a brief description of the PR_

### New MCP Server Checklist

- [ ] Not applicable. This PR is regarding MCP tools
- [ ] I have created a directory that contains my servers
- [ ] Each of my servers are their own c# project
- [ ] Each of my project names start with `MeldRx.Community.McpServers.{Your Identifier}.`
- [ ] Each of my servers have the option of connecting to it openly to verify tools
      can be listed
- [ ] Each of my servers has an accompanying README file to explain its purpose
      and other details
- [ ] I ran the csharpier formatting tool

### New MCP Tool(s) Checklist

- [ ] Not applicable. This PR is regarding MCP servers
- [ ] I have created a directory that contains my tools and services & it is pascal-cased
- [ ] All my tools implement the `IMcpTool` interface
- [ ] I have updated `ServiceCollectionExtensions.cs` that registers my tools and
      services in a private extension method
- [ ] My tools are not registered as a singleton
- [ ] I ran the csharpier formatting tool
