# Httpd
  Features:
  * __Methode GET__: Va chercher le contenu demander.
  * __Methode POST__: Recois des valeur recu dans un form et les place dans un DICTIONARY(Ne sont pas utiliser dans mon cas).
  * __App.Config__: Dans le App.Config il se trouve le port choisi(default 3k), Directory Listing, les fichiers supporter et les routes supporter.
  * __Recherche index.html__: S'il n'y a pas de path specifier, le serveur va chercher pour l'index.html.
  * __Directory Listing__: S'il n'y a pas d'index.html et que le Directory Listing est activer, une page montrant tous les fichiers apparaitra.
  * __Logging__: Le serveur Log tous les actions recu et envoyer et les mets dans un fichier qui change a chaque jour.
  * __GZIP__: Le serveur supporte l'encoding GZIP si le navigateur le supporte.
  * __Parametres des requetes__: Vous pouvez passer des parametres a la requete, ils sont storer dans un DICTIONARY et n'apparaissent seulement lorsque la route debug est active.
  Route Supporter:
  * __/debug__: Affiche le debug page.
  * __/page404__: Affiche ma page 404.
  * __/hello__: Affiche une surpise.
  
![image](https://user-images.githubusercontent.com/53085640/161177137-5bb32412-d328-4f8b-bd65-4bd02f86a385.png)

