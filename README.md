# Tower Defense J_D
Zombie Defense: City Siege
Welcome to J_D's zombie defense:, a tower defense game where your goal is simple: protect the city from waves of relentless zombies and of course have tons of fun!. 

🧟 About the Game
In this project you're tasked with strategically placing defense units to stop the zombies from destroying the city. Each wave gets a bit harder.

🛠️ Easy-to-Extend Unit System (For Developers)
The game includes a modular system for creating and customizing new towers, designed with flexibility in mind. Here’s how you can add your own towers:

🔧 How to Add a New Unit
Start with the TowerHolder prefab – drag in your own model or rig.

If the tower has custom behavior, add an EffectScript to the TowerHolder.

In the Inspector, go to the Effects & Targeting section.

Link the GameObject that should receive the event.

Select which function should be called when the tower attacks.

This setup allows you to rapidly prototype new towers without writing additional scripts for every unique action.

⚠️ UnityEvent vs Delegates: Runtime Function Binding
One technical challenge we faced was that UnityEvents require linked methods to match a specific signature. If your method takes a parameter (like a target), Unity doesn't allow runtime assignment if that parameter is exposed in the Inspector.

To work around this, we used a lightweight function lookup system:

Each custom function is registered in a small dictionary.

At runtime, the linked UnityEvent is "cloned."

The actual target method is fetched and manually added to the new event.

This lets us inject the needed parameters while still using UnityEvents for Inspector usability.

💡 Code Snippets
Here’s where you could add relevant snippets in your README (or link out to full examples):

🔹 1. Registering functions
### 🖼️ System Overview Screenshots

#### 📂 Function Dictionary & Structure  
[![Function Dictionary Setup](images/SimpleDataBase.png)](images/SimpleDataBase.png)  
*Shows the structure of the functions and their naming in the simple database.*

#### 🔁 Fetching & Referencing the Function  
[![Function Fetching Logic](Images/ConvertToString.png)](Images/ConvertToString.png)  
*Demonstrates how the function is fetched and referenced at runtime.*

#### 🧩 System in Action  
[![System In Action](images/GetString.png)](images/GetString.png)  
*Shows everything working together during gameplay.*





💡 Inspiration
The idea started after spotting an asset on the Unity Asset Store. It sparked the whole concept and grew into what you're seeing now.

🎓 School Project Background
This game was developed as part of a 9-week school project (we did it in about two weeks) focused on building a small, playable game using proper workflow, comunication and proper usage of github. The assignment was to create a tower defense game—something simple but fun, inspired by classics like Warcraft III and Plants vs. Zombies.

We worked in a small duo and focused on applying clean code practices, experimenting with new techniques, and building a few custom systems (like our modular tower setup). The goal was to learn by doing—and build something we’d actually want to play.