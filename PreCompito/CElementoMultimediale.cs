using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace PreCompito
{
    public class CLettoreMultimediale
    {
        private CElementoMultimediale[] media;
        private CElementoMultimediale scelto;

        public CLettoreMultimediale()
        {
            media = new CElementoMultimediale[5];
            InserimentoMedia();
        }

        private void InserimentoMedia()
        {
            for (var i = 0; i < media.Length; i++)
            {
                CElementoMultimediale elementoMultimediale = null;
                
                Console.Write("Inserisci tipo di media: ");
                switch (Console.ReadLine().ToLower())
                {
                    case "filmato":
                        elementoMultimediale = InserimentoFilmato();
                        break;
                    case "audio":
                        elementoMultimediale = InserimentoAudio();
                        break;
                    case "immagine":
                        elementoMultimediale = InserimentoImmagine();
                        break;
                    default:
                        Console.WriteLine("Input non valido! Deve essere: filmato, audio, immagine.");
                        i--;
                        break;
                }
                
                if (elementoMultimediale != null)
                    media[i] = elementoMultimediale;
            }
        }

        private CFilmato InserimentoFilmato()
        {
            string titolo;
            int durata;
            
            Console.Write("Inserisci titolo: ");
            titolo = Console.ReadLine();

            do
            {
                Console.Write("Inserisci durata: ");
            } while (!int.TryParse(Console.ReadLine(), out durata));
            
            return new CFilmato(titolo, durata);
        }

        private CRegistrazioneAudio InserimentoAudio()
        {
            string titolo;
            int durata;
            
            Console.Write("Inserisci titolo: ");
            titolo = Console.ReadLine();

            do
            {
                Console.Write("Inserisci durata: ");
            } while (!int.TryParse(Console.ReadLine(), out durata));
            
            return new CRegistrazioneAudio(titolo, durata);
        }

        private CImmagine InserimentoImmagine()
        {
            string titolo;
            
            Console.Write("Inserisci titolo: ");
            titolo = Console.ReadLine();
            
            return new CImmagine(titolo);
        }

        public string ShowMedia(bool filmatiOnly)
        {
            var r = string.Empty;
  
            r += "Lista media:\n";
            for (var i = 0; i < media.Length; i++)
            {
                if (!filmatiOnly)
                    r += $"[{i}] - {media[i].ToString()}\n";
                else if (filmatiOnly && media[i] is CFilmato && media[i] != scelto)
                    r += $"[{i}] - {media[i].ToString()}\n";
            }

            r += "Scelta: ";

            return r;
        }

        public void AzioniMedia(int entry)
        {
            scelto = media[entry];

            switch (scelto)
            {
                case CFilmato:
                    ShowOptions(0);
                    break;
                case CRegistrazioneAudio:
                    ShowOptions(1);
                    break;
                case CImmagine:
                    ShowOptions(2);
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }
        }
        
        public void ShowOptions(int tipo)
        {
            var r = string.Empty;
            int max = 2,
                scelta;
            
            r = "Lista opzioni:\n";
            switch (tipo)
            {
                case 0:
                    r += "[0] - Aumenta Volume\n";
                    r += "[1] - Diminuisci Volume\n";
                    r += "[2] - Aumenta Luminosita\n";
                    r += "[3] - Diminuisci Luminosita\n";
                    r += "[4] - Riproduci\n";
                    r += "[5] - Confronta\n";
                    max = 5;
                    break;
                case 1:
                    r += "[0] - Aumenta Volume\n";
                    r += "[1] - Diminuisci Volume\n";
                    r += "[2] - Riproduci\n";
                    break;
                case 2:
                    r += "[0] - Aumenta Luminosita\n";
                    r += "[1] - Diminuisci Luminosita\n";
                    r += "[2] - Mostra\n";
                    break;
            }

            r += "Scelta: ";

            Console.Write(r);
            
            while (!int.TryParse(Console.ReadLine(), out scelta) || scelta > max || scelta < 0)
                Console.WriteLine("Scelta errata!");

            Console.Write(scelto.RegistraInput(scelta));

            if (scelta != 5)
            {
                while (!int.TryParse(Console.ReadLine(), out scelta))
                    Console.WriteLine("Scelta errata!");
            
                Console.WriteLine(scelto.ConfermaInput(scelta));
            }
            else
            {
                int otherFilmato;
                
                Console.Write(ShowMedia(true));
                
                while (!int.TryParse(Console.ReadLine(), out otherFilmato) || otherFilmato > 5 || otherFilmato < 0 || !(media[otherFilmato] is CFilmato) || scelta == otherFilmato)
                {
                    Console.WriteLine("Scelta errata!");
                    Console.Write("Scelta: ");
                }
                
                switch ((scelto as CFilmato).CompareTo(media[otherFilmato] as CFilmato))
                {
                    case -1:
                        Console.WriteLine($"Primo filmato dura meno dell'altro filmato.");
                        break;
                    case 0:
                        Console.WriteLine($"I due filmati sono uguali di durata.");
                        break;
                    case 1:
                        Console.WriteLine($"Primo filmato dura più dell'altro filmato.");
                        break;
                } 
            }
        }
    }
    
    public abstract class CElementoMultimediale(string titolo)
    {
        protected string titolo = titolo;
        
        public abstract string ToString();
        public abstract string Play();
        public abstract string Show();
        public abstract string RegistraInput(int input);
        public abstract string? ConfermaInput(int input);
    }

    public class CFilmato(string titolo, int durata) : CElementoMultimediale(titolo), IVolumeControls, IBrightnessControls, IEquatable<CFilmato>, IComparable<CFilmato>
    {
        int volume = 10,
            luminosita = 5,
            durata = durata,
            modifica;

        public void Louder(int amount) 
        {
            volume += amount;
            if (volume > 20)
                volume = 10;
        }

        public void Weaker(int amount) 
        {
            volume -= amount;
            if (volume < 0)
                volume = 0;
        }

        public void Brighter(int amount) 
        {
            luminosita += amount;
            if (luminosita > 10)
                luminosita = 10;
        }

        public void Darker(int amount) 
        {
            luminosita -= amount;
            if (luminosita < 0)
                luminosita = 0;
        }

        public override string Play() 
        {
            var r = string.Empty;
            const char repeatVolume = '!',
                       repeatBrightness = '*';
            for (var i = 0; i < durata; i++)
            {
                r += titolo + new string(repeatVolume, volume) + new string(repeatBrightness, luminosita) + '\n';
            }

            return r;
        }

        public override string Show()
        {
            throw new NotImplementedException();
        }

        public bool Equals(CFilmato other)
        {
            if (other == null)
                return false;
            return this.titolo == other.titolo && this.durata == other.durata;
        }

        public override string ToString()
        {
            return $"Titolo: {titolo}, Durata: {durata}, Volume: {volume}, Luminosità: {luminosita}";
        }

        public int CompareTo(CFilmato other)
        {
            if (durata < other.durata)
                return -1;
            if (durata == other.durata)
                return 0;
            return 1;
        }

        public override string RegistraInput(int input)
        {
            var r = "Inserisci quantità: ";
            
            switch (input)
            {
                case 0:
                    modifica = 0;
                    break;
                case 1:
                    modifica = 1;
                    break;
                case 2:
                    modifica = 2;
                    break;
                case 3:
                    modifica = 3;
                    break;
                case 4:
                    r = "Conferma? ";
                    modifica = 4;
                    break;
                case 5:
                    r = "Scegliere filmato: ";
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return r;
        }

        public override string? ConfermaInput(int input)
        {
            switch (modifica)
            {
                case 0:
                    Louder(input);
                    break;
                case 1:
                    Weaker(input);
                    break;
                case 2:
                    Brighter(input);
                    break;
                case 3:
                    Darker(input);
                    break;
                case 4:
                    if (input == 1)
                        return Play();
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return null;
        }
    }

    public class CRegistrazioneAudio(string titolo, int durata) : CElementoMultimediale(titolo), IVolumeControls
    { 
        int volume = 10,
            durata = durata,
            modifica;

        public void Louder(int amount)
        {
            volume += amount;
            if (volume > 20)
                volume = 10;
        }

        public void Weaker(int amount)
        {
            volume -= amount;
            if (volume < 0)
                volume = 0;
        }

        public override string Play()
        {
            var r = string.Empty;
            const char repeatVolume = '!';
            for (var i = 0; i < durata; i++)
            {
                r += titolo + new string(repeatVolume, volume) + '\n';
            }

            return r;
        }

        public override string Show()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Titolo: {titolo}, Durata: {durata}, Volume: {volume}";
        }
        
        public override string RegistraInput(int input)
        {
            var r = "Inserisci quantità: ";
            
            switch (input)
            {
                case 0:
                    modifica = 0;
                    break;
                case 1:
                    modifica = 1;
                    break;
                case 2:
                    r = "Conferma? ";
                    modifica = 2;
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return r;
        }

        public override string? ConfermaInput(int input)
        {
            switch (modifica)
            {
                case 0:
                    Louder(input);
                    break;
                case 1:
                    Weaker(input);
                    break;
                case 2:
                    if (input == 1)
                        return Play();
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return null;
        }
    }

    public class CImmagine(string titolo) : CElementoMultimediale(titolo), IBrightnessControls
    {
        int luminosita = 5,
            modifica;

        public void Brighter(int amount)
        {
            luminosita += amount;
            if (luminosita > 10)
                luminosita = 10;
        }

        public void Darker(int amount)
        {
            luminosita -= amount;
            if (luminosita < 0)
                luminosita = 0;
        }

        public override string Show()
        {
            const char repeatBrightness = '*';
            return new string(repeatBrightness, luminosita);
        }

        public override string Play()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Titolo: {titolo}, Luminosità: {luminosita}";
        }
        
        public override string RegistraInput(int input)
        {
            var r = "Inserisci quantità: ";
            
            switch (input)
            {
                case 0:
                    modifica = 0;
                    break;
                case 1:
                    modifica = 1;
                    break;
                case 2:
                    r = "Conferma? ";
                    modifica = 2;
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return r;
        }

        public override string? ConfermaInput(int input)
        {
            switch (modifica)
            {
                case 0:
                    Brighter(input);
                    break;
                case 1:
                    Darker(input);
                    break;
                case 2:
                    if (input == 1)
                        return Show();
                    break;
                default:
                    throw new ArgumentException("Input non valido!");
            }

            return null;
        }
    }
}
