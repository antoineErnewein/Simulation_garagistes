TP : Simulation de garagistes
Environnement : WEB .NET (WebForms ou MVC)

REGLES METIER :

-	Les garagistes travaillent 8h / jour les jours ouvrés
-	Les garagistes peuvent avoir des périodes de fermeture durant l’année

-	Les garagistes procèdent aux révisions des voitures 

-	Les voitures doivent effectuer les révisions imposées par leur carnet d’entretien

-	Une révision possède les caractéristiques suivantes :
o	Libellé (ex : Vidange)
o	Kilométrage (ex : 15000)
o	Durée d’intervention (ex : 1h) (peut être différente par garagiste)

-	Les voitures roulent tous les jours

-	Les garagistes n'acceptent plus de voitures s'ils sont complets

-	Les voitures roulent au hasard entre 20 et 50 kms / jour ouvré et entre 50 et 100 kms / jour le week-end

-	Lorsqu'elles ont atteint un entretien, elles ne roulent plus jusqu'à qu'elles soient réparées

-	Les garages acceptent toutes les voitures jusqu'à ce que leur agenda du jour soit rempli

-	Des réparations peuvent être terminées le lendemain

-	Pouvoir lancer la simulation avec un nombre paramétrable de voitures et garagistes

-	Les voitures sont créées avec un kilométrage aléatoire entre 20 000 et 200 000 kms

BACK OFFICE : 
-	Back office permettant de gérer :
o	Voitures (marque/modèle)
o	Garagistes (franchise, périodes de fermetures)
o	Carnet d’entretien par type de voiture (marque et/ou modèle)

-	Les données sont stockées dans une base de données (SQL, SQLite,…).
-	Entity Framework est utilisé comme ORM pour modéliser les classes de domaines

FRONT OFFICE :
-	Front office permettant de lancer une simulation :
o	Nombre de voiture de chaque marque et/ou modèle
o	Nombre de garagistes de chaque franchise
o	Date de départ de la simulation
o	Durée en jours de la simulation

o	Un « log » en ligne pour voir le résultat de la simulation
o	Les logs sont archivés en fichier texte
o	Un rapport détaillé avec des statistiques (durant la simulation) :
o	de révisions
o	d’occupation des garagistes
o	Les résultats du rapport sont stockés en base de données

FACULTATIF :
-	Gestion d’évènements aléatoires durant la simulation (accidents, pannes, …)
