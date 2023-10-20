
using WebsiteDemos;

class Program
{
	static void Main()
	{
		// Nome do arquivo PDF de entrada
		string inputFile = "input.pdf";
		
		Console.WriteLine("Executar o ItextKernel");
		ItextKernel.Executar(inputFile);
	}
}