# Httpd
  Features:
  * __Methode GET__: Recherche le contenu demander.
  * __Methode POST__: reçoit dès valeur reçut dans un FORM et les place dans un DICTIONARY(Ne sont pas utilisés dans mon cas).
  * __App.Config__: Dans le App.Config il se trouve le port choisi(default 3k), Directory Listing, les fichiers supporter et les routes supporter.
  * __Recherche index.html__: S'il n'y a pas de path specifier, le serveur va chercher pour l'index.html.
  * __Directory Listing__: S'il n'y a pas d'index.html et que le Directory Listing est activé, une page montrant tous les fichiers apparaîtra.
  * __Logging__: Le serveur Log tous les actions reçut et envoyer et les mets dans un fichier qui change à chaque jour.
  * __GZIP__: Le serveur supporte l'encodé GZIP si le navigateur le supporte.
  * __Parametres des requetes__: Vous pouvez passer des paramètres à la requête, ils sont storer dans un DICTIONARY et n'apparaissent seulement pas lorsque la route debug est active.
  Route Supporter:
  * __/debug__: Affiche le debug page.
  * __/page404__: Affiche ma page 404.
  * __/hello__: Affiche une surpise.
  
![image](https://user-images.githubusercontent.com/53085640/161177137-5bb32412-d328-4f8b-bd65-4bd02f86a385.png)

