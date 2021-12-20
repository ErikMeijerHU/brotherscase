# Social Brothers C# Api Case<br>
Dit is het C# Api Case project gemaakt door Erik Meijer.<br>
Ik had het heel erg druk de afgelopen week dus ik heb niet heel veel tijd gehad.<br>
Dit project heeft mij in totaal ongeveer 6-8 uur gekost (grotendeels onderzoek) tussen school projecten en Albert Heijn werk door.<br>
## Gebruiksaanwijzing
De makkelijkste manier om dit project te runnen is door het in Visual Studio in te laden en dan op het groene pijltje met IIS Express te klikken.
<br><br>Dit opent automatisch een swagger window in de browser, waarin alle API calls getest kunnen worden.
### Get functie om afstand tussen twee adressen te berekenen
* Vul in de twee velden de ID's van de twee adressen waar je de afstand tussen wilt berekenen.
* Het getal dat je terug krijgt is de hoeveelheid kilometers.
* De ID's kan je vinden door de Get All functie onder deze te runnen.
### Get functie met filtering en sorting
* Laat alle velden leeg om alle adressen op te halen.
* Vul iets in in een van de velden om daar op te sorteren, je kan ook meerdere velden tegelijk.
* Vul de manier hoe je de resultaten wilt sorteren in OrderBy, default is sorteren op ID.
* Voorbeeld voor sorteren: housenumber desc,town,country.
### De andere functies zijn vrij vanzelfsprekend.
## Wat was goed, wat kon beter
### Goed:
* Ik ben zelf best wel trots op de functie voor het berekenen van afstand, het is een vrij simpele functie geworden uiteindelijk, door het gebruik van een makkelijke API en handige packages kon ik het vrij makkelijk en kort maken.
* De sorteerfunctie vind ik ook een goede, sorteerfuncties kunnen vaak heel groot zijn met veel loops en if's, maar ik heb hier een oplossing gevonden waardoor het best wel efficiënt gebeurt zonder veel onnodige loops en checks.
### Minder goed:
* De filter functie is vrij kort, maar bestaat voornamelijk uit een grote if statement in een lambda functie, omdat er nu voor elke database entry eerst gekeken word of de parameter leeg is en daarna of de waarde van de parameter in de database entry voorkomt. Dit is qua code makkelijk en kort, maar dit kan voor hele grote databases voor snelheidsproblemen zorgen. Beter zou zijn om van tevoren alle lege parameters uit de filter te halen, zodat er niet overbodige checks worden gedaan.
* Als ik meer tijd had gehad had ik alle controller functies via een repository gedaan in plaats van alleen de ingewikkeldere, meer dingen aan elkaar verbinden door interfaces voor lange termijn onderhoudbaarheid en ook had ik meer willen doen met response en error afhandeling.
