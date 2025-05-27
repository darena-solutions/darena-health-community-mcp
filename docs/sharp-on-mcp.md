# Scope and Components

The scope of the Standardized Healthcare Agent Remote Protocol (SHARP) is designed for future expansion.

**Remote Execution Paradigm:**
SHARP-on-MCP is explicitly designed for remote, server-hosted execution leveraging Server-Sent Events (SSE) or Streamable HTTP mechanisms for efficient data exchange. This design choice emphatically aligns with the "Remote" aspect embedded within the SHARP acronym, and deliberately excludes support for local stdio (standard input/output) execution models.

**Extensibility Beyond FHIR:**
Although FHIR integration represents the primary initial use case, the SHARP-on-MCP specification is intentionally designed for extensibility. It supports seamless integration with a broad spectrum of other healthcare and research data sources and services, anticipating future needs and diverse application requirements.

**Core Components and Principles:**

Inspired by the robust architectural principles behind established standards such as SMART on FHIR and CDS Hooks, the SHARP-on-MCP initiative comprises the following key components:

- SHARP-on-MCP Specification: A comprehensive draft specification detailing the protocol for remote MCP server communication and interaction.
- MCP Server Templates: Reference implementations and foundational code templates for MCP servers, provided in multiple programming languages to accelerate developer adoption and streamline implementation.
- A Testing Harness: A dedicated testing harness, provided by MeldRx, designed to facilitate rigorous validation and ensure compliance of developed MCP servers with the SHARP-on-MCP specification.
  
Collectively, these integrated components aim to empower developers to rapidly build, validate, and deploy modular, interoperable MCP servers. This infrastructure will support a wide range of sophisticated Large Language Model (LLM)-driven applications, fostering innovation and enhancing capabilities across the healthcare ecosystem.
