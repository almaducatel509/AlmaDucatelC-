using System;

class Program
{
    static void Main()
    {
        DebtManager manager = new DebtManager();
        manager.LoadDebtsFromFile(); // Charge les dettes depuis le fichier

        bool quit = false; // Variable pour gérer la sortie du programme

        while (!quit)
        {
            Console.Clear();
            Console.WriteLine("Gestion des Dettes - Menu Principal");
            Console.WriteLine("A. Enregistrer une dette");
            Console.WriteLine("B. Afficher toutes les dettes et le montant total de la dette");
            Console.WriteLine("C. Afficher la dette la plus ancienne et la plus récente");
            Console.WriteLine("D. Afficher les dettes enregistrées dans une date donnée");
            Console.WriteLine("E. Augmenter de 4% les dettes entre 50,000 et 125,000 Gourdes");
            Console.WriteLine("F. Supprimer toutes les dettes enregistrées dans la collection " +
                "suivant une date saisie par l’utilisateur.");
            Console.WriteLine("G. Restaurer une dette supprimée par son code");
            Console.WriteLine("H. Restaurer toutes les dettes");
            Console.WriteLine("Q. Quitter");

            Console.Write("Choisissez une option (A-H ou Q) : ");
            string choice = Console.ReadLine().ToUpper();

            switch (choice)
            {
                case "A":
                    manager.RecordDebt(); // Appel de la méthode pour enregistrer une dette
                    break;
                case "B":
                    manager.DisplayAllDebtsAndTotal(); // Appel de la méthode pour afficher toutes les dettes
                    break;
                case "C":
                    manager.DisplayOldestAndNewestDebts(); // Appel de la méthode pour afficher les dettes les plus anciennes et récentes
                    break;
                case "D":
                    manager.DisplayDebtsByDate(); // Appel de la méthode pour afficher les dettes enregistrées dans une date donnée
                    break;
                case "E":
                    manager.IncreaseDebtsBy4Percent(); // Appel de la méthode pour augmenter les dettes de 4%
                    break;
                case "F":
                    manager.DeleteDebtsByDate(); // Appel de la méthode pour supprimer les dettes enregistrées avant une date donnée
                    break;
                case "G":
                    manager.RestoreDebtByCode(); // Appel de la méthode pour restaurer une dette supprimée par son code
                    break;
                case "H":
                    manager.RestoreAllDebts(); // Appel de la méthode pour restaurer toutes les dettes
                    break;
                
                case "Q":
                    quit = true; // Quitter le programme
                    manager.SaveDebtsToFile(); // Sauvegarde les dettes dans le fichier
                    break;
                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option valide en choisissant une option (A-I ou Q).");
                    break;
            }
        }

        Console.WriteLine("Merci d'avoir utilisé le programme de la caisse Populaire Fraternité de LIMONADE. Au revoir !");
    }
}
