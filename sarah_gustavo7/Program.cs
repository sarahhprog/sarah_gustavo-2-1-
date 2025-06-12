using System;
using System.Linq.Expressions;
using System.Security.Authentication;

class Program
{
    // Variáveis para armazenar os dados dos veículos que entram e saem
    static string[] placas, modelos, cores, nomes;
    static string[] Privativas, Prioritarias, Comuns;
    static string[] DadosCarroEntrada = new string[5]; // 0 - placa, 1 - modelo, 2 - cor, 3 - nome, 4 - vaga.

    static string[] DadosCarroSaida = new string[5]; // 0 - placa, 1 - modelo, 2 - cor, 3 - nome, 4 - vaga.

    static int Posiçao = 0, ContPriv = 0, ContPrio = 0, ContCom = 0;
    static void Main(string[] args)
    {
        StreamReader Arquivo = new StreamReader("estacionamento_in.txt");
        StreamWriter ArquivoSaida = new StreamWriter("estacionamento_out.txt");

        string estacionamento = Arquivo.ReadLine();

        // Definição do número inicial de vagas por tipo

        int vagasTipos = int.Parse(Arquivo.ReadLine());
        Privativas = new string[vagasTipos];

        vagasTipos = int.Parse(Arquivo.ReadLine());
        Prioritarias = new string[vagasTipos];

        vagasTipos = int.Parse(Arquivo.ReadLine());
        Comuns = new string[vagasTipos];

        placas = new string[Comuns.Length + Prioritarias.Length + Privativas.Length]; modelos = new string[Comuns.Length + Prioritarias.Length + Privativas.Length]; cores = new string[Comuns.Length + Prioritarias.Length + Privativas.Length];
        nomes = new string[Comuns.Length + Prioritarias.Length + Privativas.Length];

        string placaSaida;

        int opcao;

        do
        {
            opcao = Menu(estacionamento);  // Chama o menu e recebe a opção selecionada

            switch (opcao)
            {
                case 1:// FeitoF
                    // Registro de entrada de veículo
                    Console.Clear();

                    Console.Write("Informe a placa do veiculo: ");
                    placas[Posiçao] = Console.ReadLine();

                    Console.Write("Informe o modelo do carro: ");
                    modelos[Posiçao] = Console.ReadLine();

                    Console.Write("Informe a cor do carro: ");
                    cores[Posiçao] = Console.ReadLine();

                    Console.Write("Informe o nome do proprietario: ");
                    nomes[Posiçao] = Console.ReadLine();

                    Console.WriteLine("Informe o tipo de vaga do carro (Privativa - 1, Comun - 2, Prioritaria - 3)");
                    string vagas = Console.ReadLine();

                    RegistrarEntrada(placas[Posiçao], modelos[Posiçao], cores[Posiçao], nomes[Posiçao], vagas);  // Chama o método para registrar a entrada

                    // Verifica e decreta o número de vagas disponíveis baseado no tipo de vaga
                    if (vagas == "1")
                    {
                        if (Posiçao < Privativas.Length)
                        {
                            Privativas[ContPriv] = placas[Posiçao];
                            Console.WriteLine("Entrada registrada");
                            ContPriv++;
                        }
                        else
                            Console.WriteLine("Não há vagas privativas");

                        Console.ReadKey();
                    }
                    else if (vagas == "2")
                    {
                        if (Posiçao < Comuns.Length)
                        {
                            Comuns[ContCom] = placas[Posiçao];
                            Console.WriteLine("Entrada registrada");
                            ContCom++;
                        }
                        else
                            Console.Write("Não há vagas Comuns");

                        Console.ReadKey();
                    }
                    else if (vagas == "3")
                    {
                        if (Posiçao < Prioritarias.Length)
                        {
                            Prioritarias[ContPrio] = placas[Posiçao];
                            Console.WriteLine("Entrada registrada");
                            ContPrio++;
                        }
                        else
                            Console.Write("Não há vagas prioritarias");

                        Console.ReadKey();
                    }

                    Posiçao++;// Variavel para saber a ultima casa livre do vetor

                    break;

                case 2:// Feito
                    // Registro de saída de veículo
                    Console.Clear();

                    int totalVeiculos = Posiçao - 1,
                    TotalVagas = Comuns.Length + Prioritarias.Length + Privativas.Length;

                    if (totalVeiculos == 0)
                    {
                        Console.WriteLine("Não há veiculos no estacionamento");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Write("Informe a placa do veiculo: ");
                        placaSaida = Console.ReadLine();

                        Console.WriteLine("Tipo de vaga do veiculo (Privativa - 1, Comun - 2, Prioritaria - 3)");
                        string vagaSaida = Console.ReadLine();

                        bool achou = false;
                        int i;

                        for (i = 0; i < placas.Length; i++)
                        {
                            if (placas[i] == placaSaida)
                            {
                                achou = true;
                                break;
                            }
                        }


                        if (achou)
                        {
                            RegistrarSaida(placaSaida, modelos[i], cores[i], nomes[i], vagaSaida);  // Chama o método para registrar a saída

                            for (int j = i; j < placas.Length - 1; j++)  //impede buracos em todos os vetores
                            {
                                placas[j] = placas[j + 1];
                                modelos[j] = modelos[j + 1];
                                cores[j] = cores[j + 1];
                                nomes[j] = nomes[j + 1];
                            }

                            // Aumenta vaga no tipo que o veiculo foi retirado
                            if (vagaSaida == "1")
                                ContPriv--;

                            else if (vagaSaida == "2")
                                ContCom--;

                            else if (vagaSaida == "3")
                                ContPrio--;



                            // Aumenta o número de vagas disponíveis após a saída do veículo
                            Posiçao--;
                        }
                        else
                        {
                            Console.WriteLine("Placa inexistente");
                            Console.ReadKey();
                        }
                    }
                    break;

                case 3:// Feito
                    // Consulta o número de vagas disponíveis
                    Console.Clear();
                    ConsultarVagas(ArquivoSaida);
                    Console.ReadKey();
                    break;

                case 4:// Feito
                    // Exibe resumo das vagas e do último veículo que entrou e saiu
                    Console.Clear();
                    ExibirResumo(ArquivoSaida);
                    Console.ReadKey();
                    break;

                case 5:// Feito
                    // Finaliza o programa
                    Console.WriteLine("Finalizando o programa...");
                    ArquivoSaida.Close();
                    Console.ReadKey();
                    break;

                default:// Feito
                    // Opção inválida
                    Console.WriteLine("Opção invalida");
                    Console.ReadKey();
                    break;
            }
        }
        while (opcao != 5);
    }

    // Função que exibe o menu de opções ao usuário e devolve a opção escolhida
    static int Menu(string nome)
    {
        int opcao;

        Console.Clear();
        Console.WriteLine($"{nome}");
        Console.WriteLine("----------------------");
        Console.WriteLine("1 - Registrar entrada");
        Console.WriteLine("2 - Registrar saída");
        Console.WriteLine("3 - Consultar vagas");
        Console.WriteLine("4 - Exibir resumo");
        Console.WriteLine("5 - Sair");
        Console.Write("Informe a opção desejada: ");
        opcao = int.Parse(Console.ReadLine());

        return opcao;
    }

    // Procedimento que registra a entrada de um veículo no estacionamento
    static void RegistrarEntrada(string placa, string modelo, string cor, string nome, string vaga)
    {
        DadosCarroEntrada[0] = placa;
        DadosCarroEntrada[1] = modelo;
        DadosCarroEntrada[2] = cor;
        DadosCarroEntrada[3] = nome;
        DadosCarroEntrada[4] = vaga;
    }

    // Procedimento que registra a saída de um veículo do estacionamento
    static void RegistrarSaida(string placa, string modelo, string cor, string nome, string vaga)
    {
        DadosCarroSaida[0] = placa;
        DadosCarroSaida[1] = modelo;
        DadosCarroSaida[2] = cor;
        DadosCarroSaida[3] = nome;
        DadosCarroSaida[4] = vaga;
    }
    // Procedimento que verifica se há e quantas são as vagas disponíveis de cada tipo exibindo na tela
    static void ConsultarVagas(StreamWriter ArquivoSaida)
    {
        int somaTotal = Posiçao - 1;

        if (somaTotal < 0)
            Console.WriteLine("Não há vagas!");

        else if (somaTotal > 0)
        {
            Console.WriteLine("Há vagas!");
            Console.WriteLine($"Ha {Privativas.Length - (ContPriv)} vagas Privativas.");
            Console.WriteLine($"Ha {Comuns.Length - (ContCom)} vagas Comuns.");
            Console.WriteLine($"Ha {Prioritarias.Length - (ContPrio)} vagas Prioritarias.");

            ArquivoSaida.WriteLine("Há vagas!");
            ArquivoSaida.WriteLine($"Ha {Privativas.Length - ContPriv} vagas Privativas.");
            ArquivoSaida.WriteLine($"Ha {Comuns.Length - ContCom} vagas Comuns.");
            ArquivoSaida.WriteLine($"Ha {Prioritarias.Length - ContPrio} vagas Prioritarias.");

        }
    }

    // Procedimento que exibe o número de veículos no estacionamento total e em cada tipo de vaga,
    // o número de vagas disponíveis de cada tipo, a placa, o modelo, a cor e o nome do proprietário
    // do último veículo que entrou e do último veículo que saiu
    static void ExibirResumo(StreamWriter ArquivoSaida)
    {

        Console.WriteLine($"O ultimo carro que entrou foi: placa - {DadosCarroEntrada[0]} \n modelo - {DadosCarroEntrada[1]} Cor -{DadosCarroEntrada[2]} \n Proprietario -{DadosCarroEntrada[3]} \n ");
        Console.WriteLine($"O ultimo carro que saiu foi: placa - {DadosCarroSaida[0]}\n modelo -{DadosCarroSaida[1]} \n Cor - {DadosCarroSaida[2]} \n Proprietario - {DadosCarroSaida[3]} \n ");
        ArquivoSaida.WriteLine($"Há {ContPriv + ContPrio + ContCom} veiculo(s) no estacionamento.");
        Console.WriteLine($"Ha {Privativas.Length - (ContPriv)} vagas Privativas disponiveis e {ContPriv} Ocupadas.");
        Console.WriteLine($"Ha {Comuns.Length - (ContCom)} vagas Comuns disponiveis e {ContCom} Ocupadas.");
        Console.WriteLine($"Ha {Prioritarias.Length - (ContPrio)} vagas Prioritarias disponiveis e {ContPrio} Ocupadas.");

        ArquivoSaida.WriteLine($"O ultimo carro que entrou foi: placa - {DadosCarroEntrada[0]} \n modelo - {DadosCarroEntrada[1]} Cor -{DadosCarroEntrada[2]} \n Proprietario -{DadosCarroEntrada[3]} \n ");
        ArquivoSaida.WriteLine($"O ultimo carro que saiu foi: placa - {DadosCarroSaida[0]}\n modelo -{DadosCarroSaida[1]} \n Cor - {DadosCarroSaida[2]} \n Proprietario - {DadosCarroSaida[3]} \n ");
        ArquivoSaida.WriteLine($"Há {ContPriv + ContPrio + ContCom} veiculo(s) no estacionamento.");
        ArquivoSaida.WriteLine($"Ha {Privativas.Length - (ContPriv)} vagas Privativas disponiveis e {ContPriv} Ocupadas.");
        ArquivoSaida.WriteLine($"Ha {Comuns.Length - (ContCom)} vagas Comuns disponiveis e {ContCom} Ocupadas.");
        ArquivoSaida.WriteLine($"Ha {Prioritarias.Length - (ContPrio)} vagas Prioritarias disponiveis e {ContPrio} Ocupadas.");

    }
}