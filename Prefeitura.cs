using System.Security.Cryptography.X509Certificates;

public class Prefeitura
{
    public int Emprega { get; set;}
    public float Salario { get; set;}
    public float CaixaInicial { get; set;}
    public int AssistePessoas { get; set;}
    public float Bolsa { get; set;}

    public override string ToString()
    {
        return $"Prefeitura -> Emprega: {Emprega}, Salário: {Salario}, Caixa: {CaixaInicial}, Assiste: {AssistePessoas}";
    }
}
