import { McpServer } from "@modelcontextprotocol/sdk/server/mcp";
import { Request } from "express";
import { IMcpTool } from "../IMcpTool";
import { z } from "zod";
import { getFhirContext, getPatientIdIfContextExists } from "../fhir-utilities";
import { createTextResponse } from "../mcp-utilities";
import axios from "axios";

type BundleEntry = {
  resource: {
    id: string;
    code: {
      coding: {
        display: string;
      }[];
    };
  };
};

type Bundle = {
  entry: BundleEntry[];
};

export class GetConditionsTool implements IMcpTool {
  registerTool(server: McpServer, req: Request) {
    server.tool(
      "get_patient_conditions",
      "Finds the conditions of a patient and returns it as an array. If patient context" +
        "already exists, then the patient id is not required. Otherwise, the patient" +
        "id is required",
      {
        patientId: z
          .string()
          .optional()
          .describe(
            "The patient id. Optional if patient context exists. Required otherwise."
          ),
      },
      async ({ patientId }) => {
        const fhirContext = getFhirContext(req);
        if (!fhirContext) {
          return createTextResponse(
            "A FHIR server url or token was not provided in the http context.",
            { isError: true }
          );
        }

        const patientIdContext = getPatientIdIfContextExists(req);
        if (!patientIdContext && !patientId) {
          return createTextResponse(
            "No patient context found, an id is required.",
            { isError: true }
          );
        }

        const response = await axios.get<Bundle>(
          `${fhirContext.url}/Condition?patient=${
            patientIdContext || patientId
          }`,
          { headers: { Authorization: `Bearer ${fhirContext.token}` } }
        );

        if (!response.data.entry?.length) {
          return createTextResponse(
            "No conditions could be found for this patient",
            { isError: true }
          );
        }

        const displayValues = response.data.entry
          .map((x) => x.resource.code.coding.map((y) => y.display))
          .reduce((a, b) => a.concat(b), []);

        return createTextResponse(JSON.stringify(displayValues));
      }
    );
  }
}

export const GetConditionsToolInstance = new GetConditionsTool();
