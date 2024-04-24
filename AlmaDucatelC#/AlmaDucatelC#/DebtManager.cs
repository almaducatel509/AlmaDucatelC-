using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class DebtManager
{
    private Dictionary<int, Debt> deletedDebtDictionary = new Dictionary<int, Debt>();
    private Dictionary<int, Debt> debtDictionary = new Dictionary<int, Debt>();
    private string fileName = "debts.txt";

    // Méthode pour charger les dettes depuis le fichier
    public void LoadDebtsFromFile()
    {
        if (File.Exists(fileName)) // Vérifie si le fichier existe
        {
            string[] lines = File.ReadAllLines(fileName); // Lire toutes les lignes du fichier

            foreach (string line in lines)
            {
                string[] data = line.Split(','); // Divise la ligne en utilisant la virgule comme séparateur

                if (data.Length >= 5)
                {
                    int debtNumber = int.Parse(data[0]);
                    string fullName = data[1];
                    decimal amount = decimal.Parse(data[2], CultureInfo.InvariantCulture);
                    int interestRate = int.Parse(data[3]);
                    DateTime registrationDate = DateTime.Parse(data[4]);

                    // Créer une instance de Debt et l'ajouter au dictionnaire
                    Debt debt = new Debt(debtNumber, fullName, amount, interestRate, registrationDate);
                    debtDictionary[debtNumber] = debt;
                }
            }
        }
    }
    
    // Méthode pour sauvegarder les dettes dans le fichier
    public void SaveDebtsToFile()
    {
        List<string> lines = new List<string>();

        foreach (var debt in debtDictionary.Values)
        {
            string line = $"{debt.DebtNumber},{debt.FullName},{debt.Amount},{debt.InterestRate},{debt.RegistrationDate}";
            lines.Add(line);
        }

        // Utiliser AppendAllLines pour ajouter les nouvelles dettes à la fin du fichier sans écraser les données existantes
        File.AppendAllLines(fileName, lines);
    }

    //A- Enregistrer
    // Méthode pour enregistrer une dette
    public void RecordDebt()
    {
        Console.Clear();
        Console.WriteLine("Enregistrer une nouvelle dette");

        int debtNumber;
        string fullName;
        decimal amount;
        int interestRate;

        // Validation de la saisie du numéro de dette
        while (true)
        {
            Console.Write("Numéro de dette : ");
            if (int.TryParse(Console.ReadLine(), out debtNumber))
            {
                if (!debtDictionary.ContainsKey(debtNumber))
                {
                    break; // Sortir de la boucle si la saisie est valide et le numéro de dette est unique
                }
                else
                {
                    Console.WriteLine("Numéro de dette déjà utilisé. Veuillez entrer un numéro de dette unique.");
                }
            }
            else
            {
                Console.WriteLine("Numéro de dette invalide. Veuillez entrer un entier valide.");
            }
        }

        // Validation de la saisie du nom complet
        Console.Write("Nom complet du client : ");
        fullName = Console.ReadLine();

        // Validation de la saisie du montant de la dette
        while (true)
        {
            Console.Write("Montant de la dette : ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount))
            {
                break; // Sortir de la boucle si la saisie est valide
            }
            else
            {
                Console.WriteLine("Montant de la dette invalide. Veuillez entrer un nombre décimal valide.");
            }
        }

        // Validation de la saisie du taux d'intérêt
        while (true)
        {
            Console.Write("Taux d'intérêt (en pourcentage) : ");
            if (int.TryParse(Console.ReadLine(), out interestRate))
            {
                break; // Sortir de la boucle si la saisie est valide
            }
            else
            {
                Console.WriteLine("Taux d'intérêt invalide. Veuillez entrer un entier valide.");
            }
        }

        // Charger toutes les dettes existantes depuis le fichier
        LoadDebtsFromFile();

        // Créer une nouvelle instance de Debt en passant les arguments nécessaires
        Debt newDebt = new Debt(debtNumber, fullName, amount, interestRate, DateTime.Now);

        debtDictionary.Add(debtNumber, newDebt);

        // Sauvegarder toutes les dettes dans le fichier
        SaveDebtsToFile();

        Console.WriteLine("La dette a été enregistrée avec succès !");
        Console.WriteLine("Appuyez sur une touche pour retourner au menu principal...");
        Console.ReadKey();
    }

    // B- Afficher toutes les dettes enregistrées dans le fichier ainsi que le montant total de la dette
    public void DisplayAllDebtsAndTotal()
    {
        // Cette méthode affichera toutes les dettes et le montant total de la dette.
        Console.Clear();
        Console.WriteLine("Liste de toutes les dettes");

        if (debtDictionary.Count == 0)
        {
            Console.WriteLine("Aucune dette enregistrée.");
        }
        else
        {
            foreach (var debt in debtDictionary.Values)
            {
                Console.WriteLine($"Numéro de dette : {debt.DebtNumber}");
                Console.WriteLine($"Nom du client : {debt.FullName}");
                Console.WriteLine($"Montant de la dette : {debt.Amount:C}");
                Console.WriteLine($"Taux d'intérêt : {debt.InterestRate}%");
                Console.WriteLine($"Date d'enregistrement : {debt.RegistrationDate}");
                Console.WriteLine("---------------------------------------");
            }

            // Calculez le montant total de la dette
            decimal totalDebtAmount = debtDictionary.Values.Sum(debt => debt.Amount + (debt.Amount * debt.InterestRate / 100));
            Console.WriteLine($"Montant total de la dette : {totalDebtAmount:C}");
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    // C- Afficher la dette la plus ancienne et la plus récente
    public void DisplayOldestAndNewestDebts()
    {
        if (debtDictionary.Count == 0)
        {
            Console.WriteLine("Aucune dette enregistrée.");
            return;
        }

        // Triez les dettes par date d'enregistrement
        var sortedDebts = debtDictionary.Values.OrderBy(debt => debt.RegistrationDate).ToList();

        // La première dette (indice 0) est la plus ancienne, et la dernière (indice Count - 1) est la plus récente
        Debt oldestDebt = sortedDebts[0];
        Debt newestDebt = sortedDebts[sortedDebts.Count - 1];

        Console.WriteLine("Dette la plus ancienne :");
        Console.WriteLine($"Numéro de dette : {oldestDebt.DebtNumber}");
        Console.WriteLine($"Nom complet du client : {oldestDebt.FullName}");
        Console.WriteLine($"Montant de la dette : {oldestDebt.Amount:C}");
        Console.WriteLine($"Taux d'intérêt : {oldestDebt.InterestRate}%");
        Console.WriteLine($"Date d'enregistrement : {oldestDebt.RegistrationDate}");

        Console.WriteLine("\nDette la plus récente :");
        Console.WriteLine($"Numéro de dette : {newestDebt.DebtNumber}");
        Console.WriteLine($"Nom complet du client : {newestDebt.FullName}");
        Console.WriteLine($"Montant de la dette : {newestDebt.Amount:C}");
        Console.WriteLine($"Taux d'intérêt : {newestDebt.InterestRate}%");
        Console.WriteLine($"Date d'enregistrement : {newestDebt.RegistrationDate}");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    // D- Afficher toutes les dettes enregistrées dans une date donnée
    public void DisplayDebtsByDate()
    {
        Console.Write("Entrez la date au format (MM/DD/YYYY) : ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            Console.WriteLine($"Dettes enregistrées le {date.ToShortDateString()} :");

            foreach (var debt in debtDictionary.Values)
            {
                if (debt.RegistrationDate.Date == date.Date)
                {
                    Console.WriteLine($"Numéro de dette : {debt.DebtNumber}");
                    Console.WriteLine($"Nom complet du client : {debt.FullName}");
                    Console.WriteLine($"Montant de la dette : {debt.Amount:C}");
                    Console.WriteLine($"Taux d'intérêt : {debt.InterestRate}%");
                    Console.WriteLine($"Date d'enregistrement : {debt.RegistrationDate.ToShortDateString()}");
                    Console.WriteLine("---------------------------------------------------");
                }
            }
        }
        else
        {
            Console.WriteLine("Date invalide. Assurez-vous d'entrer une date au format (MM/DD/YYYY).");
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }


    // E- Augmenter de 4% toutes les dettes de la collection dont le montant dette est compris entre 50,000 à 125,000 Gourdes
    public void IncreaseDebtsBy4Percent()
    {
        Console.WriteLine("Augmentation de 4% des dettes entre 50,000 et 125,000 Gourdes :");

        foreach (var debt in debtDictionary.Values)
        {
            if (debt.Amount >= 50000 && debt.Amount <= 125000)
            {
                // Calculer la nouvelle dette avec l'augmentation de 4%
                decimal increasedAmount = debt.Amount * 1.04M; // Multiplier par 1.04 pour augmenter de 4%

                // Mettre à jour le montant de la dette dans le dictionnaire
                debt.Amount = increasedAmount;
                Console.WriteLine($"Numéro de dette : {debt.DebtNumber}");
                Console.WriteLine($"Nouveau montant de la dette : {debt.Amount:C}");
                Console.WriteLine("---------------------------------------");
            }
        }

        // Enregistrez les modifications dans le fichier texte
        SaveDebtsToFile();
        Console.WriteLine("Augmentation terminée. Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    // F- Supprimer toutes les dettes enregistrées dans la collection suivant une date saisie par l’utilisateur.
    public void DeleteDebtsByDate()
    {
        Console.Clear();
        Console.WriteLine("Supprimer les dettes par date");

        // Demandez à l'utilisateur de saisir une date au format (MM/DD/YYYY)
        Console.Write("Entrez une date au format (MM/DD/YYYY) : ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime inputDate))
        {
            List<Debt> debtsToDelete = new List<Debt>();

            // Parcourez la collection de dettes
            foreach (var debt in debtDictionary.Values)
            {
                if (debt.RegistrationDate.Date == inputDate.Date && !debt.IsDeleted)
                {
                    debtsToDelete.Add(debt); // Ajoutez la dette à la liste debtsToDelete
                }
            }

            if (debtsToDelete.Count > 0)
            {
                Console.WriteLine("Dettes trouvées pour la date saisie :");
                foreach (var debt in debtsToDelete)
                {
                    Console.WriteLine($"Numéro de dette : {debt.DebtNumber}");
                    Console.WriteLine($"Montant de la dette : {debt.Amount:C}");
                    Console.WriteLine($"Date de la dette : {debt.RegistrationDate}");
                }

                // Demandez à l'utilisateur de confirmer la suppression
                Console.Write("Voulez-vous supprimer ces dettes ? (oui/non) : ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "oui")
                {
                    // Marquez les dettes comme supprimées (set IsDeleted à true)
                    foreach (var debt in debtsToDelete)
                    {
                        debt.IsDeleted = true;
                    }

                    // Enregistrez les modifications dans le fichier texte
                    SaveDebtsToFile();

                    Console.WriteLine("Dettes supprimées avec succès.");
                }
                else
                {
                    Console.WriteLine("Suppression annulée.");
                }
            }
            else
            {
                Console.WriteLine("Aucune dette non supprimée trouvée pour la date saisie.");
            }
        }
        else
        {
            Console.WriteLine("Date invalide. Assurez-vous d'entrer une date au format (MM/DD/YYYY).");
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }


    // G- Restaurer une dette supprimée par son code
    public void RestoreDebtByCode()
    {
        Console.Clear();
        Console.WriteLine("Dettes supprimées");

        List<Debt> deletedDebts = new List<Debt>();

        // Parcourez la collection de dettes pour trouver celles marquées comme supprimées
        foreach (var debt in debtDictionary.Values)
        {
            if (debt.IsDeleted)
            {
                deletedDebts.Add(debt);
            }
        }

        if (deletedDebts.Count > 0)
        {
            Console.WriteLine("Dettes supprimées trouvées :");
            foreach (var debt in deletedDebts)
            {
                Console.WriteLine($"Numéro de dette : {debt.DebtNumber}");
                Console.WriteLine($"Montant de la dette : {debt.Amount:C}");
                Console.WriteLine($"Date de la dette : {debt.RegistrationDate}");
                Console.WriteLine($"Supprimée : Oui");
            }

            // Demandez à l'utilisateur s'il souhaite restaurer des dettes
            Console.Write("Voulez-vous restaurer des dettes supprimées ? (oui/non) : ");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "oui")
            {
                // Restaurez les dettes en les marquant comme non supprimées
                foreach (var debt in deletedDebts)
                {
                    debt.IsDeleted = false;
                }

                // Enregistrez les modifications dans le fichier texte
                SaveDebtsToFile();

                Console.WriteLine("Dettes restaurées avec succès.");
            }
            else
            {
                Console.WriteLine("Opération annulée.");
            }
        }
        else
        {
            Console.WriteLine("Aucune dette supprimée trouvée.");
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }


    // H- Restaurer toutes les dettes
    public void RestoreAllDebts()
    {
        foreach (var deletedDebt in deletedDebtDictionary.Values)
        {
            debtDictionary[deletedDebt.DebtNumber] = deletedDebt;
        }

        deletedDebtDictionary.Clear(); // Effacer toutes les dettes supprimées

        // Enregistrez les modifications dans le fichier texte
        SaveDebtsToFile();
        Console.WriteLine("Toutes les dettes restaurées avec succès !");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }


}
