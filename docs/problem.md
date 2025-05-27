# The Problem We're Solving

While the Model Context Protocol (MCP) has emerged as a powerful standard for connecting Large Language Models (LLMs) to external tools, APIs, and data sources, a critical gap exists: there's no defined standard for building reusable and pluggable remote MCP servers within the healthcare domain.

Current observations highlight several key challenges:

- Fragmented FHIR-based Development: Healthcare-focused MCP servers are predominantly designed to interact with FHIR data. However, current development efforts are fragmented. This presents a significant opportunity to consolidate around standardized, reusable FHIR-based MCP tools that aren't tightly coupled to any single FHIR server instance.

- Tight Coupling of MCP Logic: Developers commonly embed MCP logic directly within FHIR servers or tightly integrate it into LLM applications. This approach leads to rigid, non-portable implementations that are difficult to reuse, scale, or maintain across different environments.

- Expanding Beyond FHIR: While FHIR remains a primary data source for clinical context, emerging healthcare use cases, such as specialized clinical trials, advanced research analytics, and sophisticated document search capabilities, demand a more flexible and extensible architectural paradigm for MCP servers that can accommodate diverse data modalities beyond FHIR.

Just as SMART on FHIR decoupled healthcare applications from Electronic Health Record (EHR) systems, SHARP-on-MCP aims to decouple remote MCP servers from specific FHIR server implementations. This decoupling is essential to foster reusability, enhance scalability, and accelerate the growth of a robust, interoperable ecosystem for AI agents in healthcare.
