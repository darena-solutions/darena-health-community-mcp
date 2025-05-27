import { McpServer } from "@modelcontextprotocol/sdk/server/mcp";
import { StreamableHTTPServerTransport } from "@modelcontextprotocol/sdk/server/streamableHttp";
import express from "express";
import * as tools from "./mcp-tools";
import { IMcpTool } from "./IMcpTool";

const app = express();
const port = 5000;

app.use(express.json());

app.get("/hello-world", async (_, res) => {
  res.status(200).send("Hello World!");
});

app.post("/", async (req, res) => {
  try {
    const server = new McpServer({
      name: "Typescript Template",
      version: "1.0.0",
    });

    for (const tool of Object.values<IMcpTool>(tools)) {
      tool.registerTool(server, req);
    }

    const transport = new StreamableHTTPServerTransport({
      sessionIdGenerator: undefined,
    });

    res.on("close", () => {
      console.log("Request closed");

      transport.close();
      server.close();
    });

    await server.connect(transport);
    await transport.handleRequest(req, res, req.body);
  } catch (error) {
    console.log("Error handling MCP request:", error);
    if (!res.headersSent) {
      res.status(500).json({
        jsonrpc: "2.0",
        error: {
          code: -32603,
          message: "Internal server error",
        },
        id: null,
      });
    }
  }
});

app.listen(port, () => {
  console.log(`MCP server listening on port ${port}`);
});
