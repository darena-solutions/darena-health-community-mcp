export function createTextResponse(
  text: string,
  options: { isError: boolean } = { isError: false }
): { content: { type: "text"; text: string }[]; isError?: boolean } {
  return {
    content: [{ type: "text", text }],
    isError: options.isError,
  };
}
