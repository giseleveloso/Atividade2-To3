using System;

class Program
{
    static void Main()
    {
        int pessoas = 1000;
        Prefeitura prefeitura = new Prefeitura();
        Comercio comercio = new Comercio();
        Industria industria = new Industria();

        // Configurando valores iniciais
        SetValores(prefeitura, comercio, industria);

        // Contadores para acompanhamento
        int anos = 0;
        float impostoComercioAnual = 0;
        float impostoIndustriaAnual = 0;
        float impostoPopulacaoAnual = 0;

        bool continuar = true;
        while (continuar)
        {
            // Zerar contadores de impostos anuais
            impostoComercioAnual = 0;
            impostoIndustriaAnual = 0;
            impostoPopulacaoAnual = 0;

            for (int mes = 1; mes <= 12; mes++) // Simula 12 meses (1 ano)
            {
                Console.WriteLine($"\nMês {mes} do Ano {anos + 1}:");

                // Executar o ciclo mensal e coletar os valores de impostos
                var resultado = CicloMensal(prefeitura, comercio, industria, pessoas);
                continuar = resultado.Continuar;

                // Acumular os impostos mensais
                impostoComercioAnual += resultado.ImpostoComercio;
                impostoIndustriaAnual += resultado.ImpostoIndustria;
                impostoPopulacaoAnual += resultado.ImpostoPopulacao;

                if (!continuar)
                {
                    Console.WriteLine("Ciclo interrompido!");
                    break;
                }
            }

            anos++;
            // Exibir informações ao final de cada ano
            Console.WriteLine($"\n========== ANO {anos} FINALIZADO ==========");
            Console.WriteLine(prefeitura);
            Console.WriteLine(comercio);
            Console.WriteLine(industria);
            Console.WriteLine($"\nImpostos pagos no ano {anos}:");
            Console.WriteLine($"Comércio: R$ {impostoComercioAnual:N2}");
            Console.WriteLine($"Indústria: R$ {impostoIndustriaAnual:N2}");
            Console.WriteLine($"População: R$ {impostoPopulacaoAnual:N2}");
            Console.WriteLine($"Total de impostos: R$ {(impostoComercioAnual + impostoIndustriaAnual + impostoPopulacaoAnual):N2}");
            Console.WriteLine("======================================");
        }

        Console.WriteLine($"\nSimulação encerrada após {anos} anos.");
    }

    struct ResultadoCiclo
    {
        public bool Continuar;
        public float ImpostoComercio;
        public float ImpostoIndustria;
        public float ImpostoPopulacao;
    }

    static ResultadoCiclo CicloMensal(Prefeitura prefeitura, Comercio comercio, Industria industria, int pessoas)
    {
        ResultadoCiclo resultado = new ResultadoCiclo();

        // 1. PAGAMENTO DOS SALÁRIOS E BENEFÍCIOS
        float totalSalarioPrefeitura = prefeitura.Emprega * prefeitura.Salario;
        float totalSalarioComercio = comercio.Emprega * comercio.Salario;
        float totalSalarioIndustria = industria.Emprega * industria.Salario;
        float totalBeneficios = prefeitura.AssistePessoas * 1000;

        Console.WriteLine($"Salários pagos - Prefeitura: R$ {totalSalarioPrefeitura:N2}, Comércio: R$ {totalSalarioComercio:N2}, Indústria: R$ {totalSalarioIndustria:N2}");
        Console.WriteLine($"Benefícios pagos: R$ {totalBeneficios:N2}");

        // Verificar se as entidades têm dinheiro para pagar salários
        if (comercio.CaixaInicial < totalSalarioComercio)
        {
            Console.WriteLine("Comércio não tem dinheiro suficiente para pagar salários!");
            resultado.Continuar = false;
            return resultado;
        }

        if (industria.CaixaInicial < totalSalarioIndustria)
        {
            Console.WriteLine("Indústria não tem dinheiro suficiente para pagar salários!");
            resultado.Continuar = false;
            return resultado;
        }

        // atualizando caixas das entidades
        prefeitura.CaixaInicial -= totalSalarioPrefeitura + totalBeneficios;
        comercio.CaixaInicial -= totalSalarioComercio;
        industria.CaixaInicial -= totalSalarioIndustria;

        // Recolhimento de impostos sobre salários
        float impostoEmpresaComercio = totalSalarioComercio * 0.61f;
        float impostoEmpresaIndustria = totalSalarioIndustria * 0.61f;
        float impostoPessoasComercio = totalSalarioComercio * 0.25f;
        float impostoPessoasIndustria = totalSalarioIndustria * 0.25f;

        // Total de impostos sobre salários
        float totalImpostoEmpresas = impostoEmpresaComercio + impostoEmpresaIndustria;
        float totalImpostoPessoas = impostoPessoasComercio + impostoPessoasIndustria;

        // Armazenar impostos para relatório
        resultado.ImpostoComercio = impostoEmpresaComercio;
        resultado.ImpostoIndustria = impostoEmpresaIndustria;
        resultado.ImpostoPopulacao = totalImpostoPessoas;

        Console.WriteLine($"Impostos sobre salários - Empresas: R$ {totalImpostoEmpresas:N2}, Pessoas: R$ {totalImpostoPessoas:N2}");

        // Adicionar impostos ao caixa da prefeitura
        prefeitura.CaixaInicial += totalImpostoEmpresas + totalImpostoPessoas;

        // 2. COMPRAS NO COMÉRCIO
        // Calcular salários líquidos (já com impostos descontados)
        float salarioLiquidoComercio = totalSalarioComercio - impostoPessoasComercio;
        float salarioLiquidoIndustria = totalSalarioIndustria - impostoPessoasIndustria;
        float totalBeneficiosLiquidos = totalBeneficios; // Beneficiários não pagam imposto sobre benefícios

        // Total de dinheiro disponível para compras
        float dinheiroGasto = salarioLiquidoComercio + salarioLiquidoIndustria + totalBeneficiosLiquidos;

        // Número de itens que podem ser comprados com o dinheiro disponível
        int itensComprados = (int)(dinheiroGasto / comercio.PrecoVenda);

        Console.WriteLine($"Dinheiro disponível para compras: R$ {dinheiroGasto:N2}");
        Console.WriteLine($"Quantidade de itens comprados: {itensComprados}");

        float receitaComercio = itensComprados * comercio.PrecoVenda;
        comercio.CaixaInicial += receitaComercio;

        // Aplicar impostos sobre as vendas do comércio
        float impostoVendasComercio = receitaComercio * 0.38f;
        prefeitura.CaixaInicial += impostoVendasComercio;
        comercio.CaixaInicial -= impostoVendasComercio;

        // Atualizar total de impostos do comércio
        resultado.ImpostoComercio += impostoVendasComercio;

        Console.WriteLine($"Receita do comércio: R$ {receitaComercio:N2}");
        Console.WriteLine($"Impostos sobre vendas: R$ {impostoVendasComercio:N2}");

        // 3. REPOSIÇÃO DO ESTOQUE DO COMÉRCIO
        int pessoasAtivas = prefeitura.Emprega + comercio.Emprega + industria.Emprega;

        // Estoque necessário para atender todas as pessoas
        int estoqueNecessario = pessoasAtivas;

        Console.WriteLine($"Estoque necessário: {estoqueNecessario} itens");

        // Verificar se é necessário repor estoque
        if (estoqueNecessario > itensComprados)
        {
            Console.WriteLine("Comércio não consegue atender toda a população!");
            resultado.Continuar = false;
            return resultado;
        }

        // Calcular custo de reposição
        float custoReposicao = estoqueNecessario * industria.PrecoVenda;

        // Verificar se o comércio tem dinheiro para repor estoque
        if (comercio.CaixaInicial < custoReposicao)
        {
            Console.WriteLine("Comércio não tem dinheiro para repor estoque!");
            resultado.Continuar = false;
            return resultado;
        }

        // Transação de reposição de estoque
        comercio.CaixaInicial -= custoReposicao;
        industria.CaixaInicial += custoReposicao;

        Console.WriteLine($"Custo de reposição: R$ {custoReposicao:N2}");

        // Aplicar impostos sobre a venda da Indústria
        float impostoVendasIndustria = custoReposicao * 0.18f;
        prefeitura.CaixaInicial += impostoVendasIndustria;
        industria.CaixaInicial -= impostoVendasIndustria;

        // Atualizar total de impostos da indústria
        resultado.ImpostoIndustria += impostoVendasIndustria;

        Console.WriteLine($"Impostos sobre vendas da indústria: R$ {impostoVendasIndustria:N2}");

        // 4. CUSTO DE PRODUÇÃO NA INDÚSTRIA
        float custoProducao = estoqueNecessario * industria.CustoProducaoItem;

        // Verificar se a Indústria tem dinheiro para produzir
        if (industria.CaixaInicial < custoProducao)
        {
            Console.WriteLine("Indústria não tem dinheiro para produzir!");
            resultado.Continuar = false;
            return resultado;
        }

        // Deduzir custos de produção
        industria.CaixaInicial -= custoProducao;

        Console.WriteLine($"Custo de produção na indústria: R$ {custoProducao:N2}");

        // Exibir saldos após o ciclo
        Console.WriteLine($"Saldos após o ciclo - Prefeitura: R$ {prefeitura.CaixaInicial:N2}, Comércio: R$ {comercio.CaixaInicial:N2}, Indústria: R$ {industria.CaixaInicial:N2}");

        // Tudo ocorreu bem
        resultado.Continuar = true;
        return resultado;
    }

    static void SetValores(Prefeitura prefeitura, Comercio comercio, Industria industria)
    {
        prefeitura.Emprega = 125;
        prefeitura.Salario = 20000;
        prefeitura.CaixaInicial = 0;
        prefeitura.AssistePessoas = 55;
        prefeitura.Bolsa = 1000;

        comercio.Emprega = 200;
        comercio.Salario = 7500;
        comercio.CaixaInicial = 10000000;
        comercio.CustoReposicao = 75;
        comercio.PrecoVenda = 203;

        industria.Emprega = 675;
        industria.Salario = 10000;
        industria.CaixaInicial = 50000000;
        industria.CustoProducaoItem = 42.75f;
        industria.PrecoVenda = 75;
    }
}
