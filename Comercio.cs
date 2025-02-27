public class Comercio
{
    public int Emprega { get; set; }
    public float Salario { get; set; }
    public float CaixaInicial { get; set; }
    public float CustoReposicao { get; set; }
    public float PrecoVenda { get; set; }

    public override string ToString()
    {
        return $"Comércio -> Emprega: {Emprega}, Salário: {Salario}, Caixa: {CaixaInicial}, Custo: {CustoReposicao}, Preço: {PrecoVenda}";
    }
}
