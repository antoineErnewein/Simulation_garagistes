TP : Simulation de garagistes
Environnement : WEB .NET (WebForms ou MVC)

REGLES METIER :

-	Les garagistes travaillent 8h / jour les jours ouvrés	[X]
-	Les garagistes peuvent avoir des périodes de fermeture durant l’année	[X]

-	Les garagistes procèdent aux révisions des Voitures 	[X]

-	Les voitures doivent effectuer les révisions imposées par leur carnet d’entretien	[X]

-	Une révision possède les caractéristiques suivantes :	[X]
o	Libellé (ex : Vidange)
o	Kilométrage (ex : 15000)
o	Durée d’intervention (ex : 1h) (peut être différente par garagiste)

-	Les voitures roulent tous les jours		[X]

-	Les garagistes n'acceptent plus de voitures s'ils sont complets		[X]

-	Les voitures roulent au hasard entre 20 et 50 kms / jour ouvré et entre 50 et 100 kms / jour le week-end	[X]

-	Lorsqu'elles ont atteint un entretien, elles ne roulent plus jusqu'à qu'elles soient réparées	[X]

-	Les garages acceptent toutes les voitures jusqu'à ce que leur agenda du jour soit rempli	[X]

-	Des réparations peuvent être terminées le lendemain		[X]

-	Pouvoir lancer la simulation avec un nombre paramétrable de voitures et Garagistes 	   [X]

-	Les voitures sont créées avec un kilométrage aléatoire entre 20 000 et 200 000 kms	   [X]

BACK OFFICE : 
-	Back office permettant de gérer :
o	Voitures (marque/modèle)	[X]
o	Garagistes (franchise, périodes de fermetures)	[X]
o	Carnet d’entretien par type de voiture (marque et/ou modèle)	[X]

-	Les données sont stockées dans une base de données (SQL, SQLite,…).		[X]
-	Entity Framework est utilisé comme ORM pour modéliser les classes de domaines	[X]

FRONT OFFICE :
-	Front office permettant de lancer une simulation :
o	Nombre de voiture de chaque marque et/ou modèle		[X]
o	Nombre de garagistes de chaque franchise	[X]
o	Date de départ de la Simulation 	[X]
o	Durée en jours de la Simulation 	[X]

-	Un « log » en ligne pour voir le résultat de la simulation 		[X]
o	Les logs sont archivés en fichier texte 	[X]
o	Un rapport détaillé avec des statistiques (durant la simulation) : 		[X]
o	de révisions
o	d’occupation des garagistes
o	Les résultats du rapport sont stockés en base de données

FACULTATIF :
-	Gestion d’évènements aléatoires durant la simulation (accidents, pannes, …)		[/]

REPARTITION :
-  Noel Deutschmann : Back office metier
-  Antoine Ernewein : Front office simulation
-  Maxime Heckel : Accès aux données
-  Etienne Heitz : Accès aux données
