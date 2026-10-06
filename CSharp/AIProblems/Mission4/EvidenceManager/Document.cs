namespace EvidenceManager;

public class Document
{
    private string CaseFileCode;

    public Document(string caseFileCode)
    {
        CaseFileCode = caseFileCode;
    }

    public string GetCaseFileCode() { return CaseFileCode; }
}
