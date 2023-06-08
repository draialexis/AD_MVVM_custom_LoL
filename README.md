
# MVVM_LoL_master_detail
  
## Instructions (FR)

Réaliser une application MAUI avec un MVVM "maison". 
Je vous fournis le modèle, et peut-être quelques vues au fur et à mesure.  
  
J'attends de vous :
- [x] la réalisation d'un toolkit MVVM (bibliothèque de classes),
- [x] le _wrapping_ des classes du modèle par des VM (à chaque fois que c'est nécessaire),
- [x] l'utilisation de commandes pour les différentes fonctionnalités,
- [x] l'utilisation d'une VM _applicative_ (navigation, index, sélection...).

Faites ce que vous pouvez avec, dans l'ordre :
- [x] l'affichage de la collection de Champions. La possibilité de naviguer de n en n champions (5 champions par page, ou 10, etc.) et la pagination doivent être gérées. 
- [x] Permettez la sélection d'un champion pour le voir dans une page (on n'utilisera que ses propriétés simples (```Name```, ```Bio```, ```Icon```) puis ```LargeImage```).
- [x] Ajoutez la gestion des caractéristiques (```Characteristics```).
- [x] Ajoutez la gestion de la classe du champion.
- [x] Permettez la modification d'un champion existant (depuis la page du champion, et depuis un swipe sur l'item sélectionné dans la collection).
- [x] Permettez l'ajout d'un nouveau champion.
- [x] Ajoutez la gestion des skills.
- [ ] ~~Ajoutez la gestion des skins.~~

## Screenshots
The app is not meant to be pretty, but the hope is that it is usable, and that the navigation experience is pleasant. At the time of this writing, it looks like this:  
<img src="./Documentation/home.png" width=200 height=450/>
<img src="./Documentation/champs.png" width=200 height=450/>
<img src="./Documentation/champ-1.png" width=200 height=450/>
<img src="./Documentation/champ-2.png" width=200 height=450/>
<img src="./Documentation/champ-edit-1.png" width=200 height=450/>
<img src="./Documentation/champ-edit-2.png" width=200 height=450/>
<img src="./Documentation/champs-swipe.png" width=200 height=450/>
<img src="./Documentation/champ-add.png" width=200 height=450/>

## Class diagrams: M, V, VM... AppVM

```mermaid
classDiagram

class  V_MainAppVM  {

+INavigation Navigation

+ICommand NavToSelectChampionCommand

+ICommand NavToAddChampionCommand

+ICommand NavToUpdateChampionCommand

+ICommand NavToAllChampionsAfterDeletingCommand

+ICommand NavToAllChampionsAfterUpsertingCommand

}

class  VM_ChampionsMgrVM  {

+ObservableCollection<ChampionVM> ChampionsVM

+ICommand LoadChampionsCommand

+ICommand InitializeCommand

+ICommand NextPageCommand

+ICommand PreviousPageCommand

+ICommand UpsertChampionFormVMCommand

+ICommand DeleteChampionCommand

}

class  VM_ChampionVM  {

+ChampionVM(clone:  ChampionVM)

}

class  VM_ChampionFormVM  {

+ICommand AddCharacteristicCommand

+ICommand UpdateCharacteristicCommand

+ICommand DeleteCharacteristicCommand

+ICommand UpsertIconCommand

+ICommand UpsertImageCommand

+ICommand AddSkillCommand

+ICommand UpdateSkillCommand

+ICommand DeleteSkillCommand

}

  

V_ChampionPage  -->  V_MainAppVM

V_ChampionPage  -->  VM_ChampionVM

V_ChampionFormPage  -->  V_MainAppVM

V_ChampionFormPage  -->  VM_ChampionFormVM

V_ChampionsPage  -->  V_MainAppVM

V_MainAppVM  -->  VM_ChampionsMgrVM

  

VM_ChampionsMgrVM  --|>  VMToolkit_PropertyChangeNotifier

VM_ChampionsMgrVM  -->  VM_ChampionVM  :  *

VM_ChampionsMgrVM  -->  M_IDataManager

VM_ChampionVM  --|>  VMToolkit_PropertyChangeNotifier

VM_ChampionVM  -->  VM_SkillVM  :  *

VM_ChampionVM  -->  M_Champion

VM_SkillVM  --|>  VMToolkit_PropertyChangeNotifier

VM_SkillVM  -->  M_Skill

VM_ChampionFormVM  -->  VM_ChampionVM

  

M_IDataManager  -->  M_Champion  :  *

M_Champion  -->  M_Skill  :  *
```