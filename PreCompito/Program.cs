using PreCompito;

CLettoreMultimediale lettoreMedia = new CLettoreMultimediale();
char continua;

Console.WriteLine("Media inseriti!");
do
{
    int scelta;
    
    Console.Write(lettoreMedia.ShowMedia(false));
    
    while (!int.TryParse(Console.ReadLine(), out scelta) || scelta > 5 || scelta < 0)
    {
        Console.WriteLine("Scelta errata!");
        Console.Write("Scelta: ");
    }
    
    lettoreMedia.AzioniMedia(scelta);
    
    Console.WriteLine("Continua? [Y/N]");
    continua = Console.ReadKey(true).KeyChar;
} while (continua == 'Y' || continua == 'y');