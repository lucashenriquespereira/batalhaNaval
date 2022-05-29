using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
					
public class Program
{
	static Dictionary<char, int> linhasArena = new Dictionary<char, int> { {'A', 0}, {'B', 1}, {'C', 2}, {'D', 3}, {'E', 4}, {'F', 5}, {'G', 6}, {'H', 7}, {'I', 8}, {'J', 9} };
	static Dictionary<string, int> tamanhoFrota = new Dictionary<string, int> { {"PS", 5}, {"NT", 4}, {"DS", 3}, {"SB", 2} };
	static int vidaPlayer1 = 30;
	static int vidaPlayer2 = 30;
	static int tamanho = 10;
	public static void Main()
	{
		Console.WriteLine("Bem vindo a Batalha Naval!");
		Console.WriteLine("Digite o nome do primeiro jogador:");
		var player1 = Console.ReadLine();
		Console.WriteLine("Digite o nome do segundo jogador:");
		var player2 = Console.ReadLine();
		string[,] arenaGabarito1 = PosicionamentoNoTabuleiro(player1);
		string[,] arenaGabarito2 = PosicionamentoNoTabuleiro(player2);
		int disparos = 200;
		string[,] campo1 = new string[tamanho,tamanho];;
		string[,] campo2 = new string[tamanho,tamanho];;
		while ( disparos > 0)
		{
			var vezPlayer1 = Combate(player1, campo1, arenaGabarito2, vidaPlayer2);
				if( vidaPlayer2 == 0)
					Console.WriteLine($"{player1} venceu esse jogo!");
			var vezPlayer2 = Combate(player2, campo2, arenaGabarito1, vidaPlayer1);
				if( vidaPlayer1 == 0)
					Console.WriteLine($"{player2} venceu esse jogo!");
		}
	}

    static string[,] PosicionamentoNoTabuleiro(string player)
	{
		Dictionary<string, int> frotaDisp = new Dictionary<string, int> { {"PS", 1}, {"NT", 2}, {"DS", 3}, {"SB", 4} };
		//int numEmbarc = 10;
		int numEmbarc = 2; //teste
		string[,] arena = new string[tamanho,tamanho];
		while (numEmbarc > 0)
		{
			var embarcacao = EscolhaEmbarcacao(player, frotaDisp);
			var posicao = EscolhaPosicao(embarcacao, arena);
			numEmbarc--;
			frotaDisp[embarcacao]--;
		}
		return arena;
	}
	
	static bool VerEspacOcupado(int linhaInicio, int linhaFim, int colunaInicio, int colunaFim, string[,] arena)
	{
		for (var i = linhaInicio; i <= linhaFim; i++)
		{
			for (var j = colunaInicio; j <= colunaFim; j++)
			{
				if (arena[i,j] != null)
				{
					return false;
				}
			}
		}
		return true;
	}
	
	static bool TamanhoEmbarcacao(int linhaInicio, int linhaFim, int colunaInicio, int colunaFim, string embarcacao) //A1B1 - coluna.Should()Be(2) - linha.Should()Be(1)
	{
		var tamanhoLinha = linhaFim - linhaInicio + 1; // result seria 1-0 = 1
		var tamanhoColuna = colunaFim - colunaInicio + 1; //result seria 1-1 = 0
		if (tamanhoLinha != tamanhoFrota[embarcacao] && tamanhoColuna != tamanhoFrota[embarcacao]) //embarcação = 2
		{
			return false;
		}
		return true;
	}
	
	static string EscolhaEmbarcacao(string player, Dictionary<string, int> frotaDisp )
	{
		bool validaEmbarcacao = false;
		string embarcacao = "";
		do
		{
			Console.WriteLine($"{player}, qual o tipo de embarcação você quer posicionar? Você tem {frotaDisp["PS"]}PS, {frotaDisp["NT"]}NT, {frotaDisp["DS"]}DS, {frotaDisp["SB"]}SB."); //PS
			embarcacao = Console.ReadLine().ToUpper();
	    	var pattern = @"(PS|NT|DS|SB)";
	    	Regex regex = new Regex(pattern);
			if (!regex.Match(embarcacao).Success)
			{
				Console.WriteLine("Embarcação inválida!");
			}
			else if (frotaDisp[embarcacao] == 0)
			{
				Console.WriteLine("Você já colocou todas as embarcações desse tipo.");
			}
			else
				validaEmbarcacao = true;
		} while (validaEmbarcacao == false);
		return embarcacao;
	}
	
	static string[,] EscolhaPosicao(string embarcacao, string[,] arena)
	{
		bool validaPosicao = false;
		string posicao = "";
		do
		{
			Console.WriteLine("Qual a posição inicial e final? Linha: A-J e Coluna 0-9");
			posicao = Console.ReadLine().ToUpper();
	    	var pattern = @"\b[A-J][0-9][A-J][0-9]\b";
	    	Regex regex = new Regex(pattern);
			if (!regex.Match(posicao).Success)
			{
				Console.WriteLine("Posição inválida!");
				continue;
			}
			var linhaInicio = linhasArena[posicao[0]];
			var colunaInicio = int.Parse(posicao.Substring(1,1));
			var linhaFim = linhasArena[posicao[2]];
			var colunaFim = int.Parse(posicao.Substring(3,1));

			if (linhaInicio != linhaFim && colunaInicio != colunaFim )
			{
				Console.WriteLine("Só pode ser colocado na vertical ou horizontal.");
				continue; //break?
			}
			else if (VerEspacOcupado(linhaInicio, linhaFim, colunaInicio, colunaFim, arena) == false)
			{
				Console.WriteLine("A posição selecionada já contém outra embarcação.");
				continue;
			}
			else if (TamanhoEmbarcacao(linhaInicio, linhaFim, colunaInicio, colunaFim, embarcacao) == false)
			{
				Console.WriteLine($"A posição selecionada não é compativel com o tamanho da embarcação, ela deve conter {tamanhoFrota[embarcacao]} espaços.");
				continue;
			}
			else
			{
				for (var i = linhaInicio; i <= linhaFim; i++) //A10C10
				{
					for (var j = colunaInicio; j <= colunaFim; j++)
					{
						arena[i,j] = embarcacao;
					}
				}
				validaPosicao = true;
			}
			//printar a arena.
		} while (validaPosicao == false);
		return arena;
	}
	
	static string[,] Combate(string player, string[,] campo, string[,] arenaGabarito, int vidaPlayer)
	{
		for (var i = 0; i <= 9; i++) //A10C10
		{
			for (var j = 0; j <= 9; j++)
			{
				Console.Write(campo[i,j]);	
			}
			Console.WriteLine();
		}
		int doWhile = 1;
		do
		{
			Console.WriteLine("Qual a coordenada que você mandará seu míssel? Linha: A-J e Coluna 0-9");
			var tryHit = Console.ReadLine().ToUpper();
	    	var pattern = @"\b[A-J][0-9]\b";
	    	Regex regex = new Regex(pattern);
			if (!regex.Match(tryHit).Success)
			{
				Console.WriteLine("Coordenada inválida!");
				continue;
			}
			var linha = linhasArena[tryHit[0]];
			var coluna = int.Parse(tryHit.Substring(1,1));
			for (var i = linha; i <= linha; i++)
			{
				for (var j = coluna; j <= coluna; j++)
				{
					if (arenaGabarito[i,j] != null)
					{
						Console.WriteLine("Você acertou!");
						campo[i,j] = "X";
						vidaPlayer--;
						
					}
					else
					{
						Console.WriteLine("Você errou!");
						campo[i,j] = "A";
					}
				}
			}
			doWhile--;
		} while (doWhile > 0);
		return campo;
	}
}
