# ToHelpPeople
This project was created to help people by letting them have a look at a simple 2D project that uses the Post Processing Stack V2, LWRP and a simple game. It's goal is to help people learn Unity, Post Processing Stack V2, LWRP, C#, or whatever else added.

## Why
I ended up getting really bored and didn't know what else to do and needed to just focus on something else so what I did was decided to help out people with programming and while being in Dani's server (A Programming YouTuber) I spoke to people who needed help and well helped them. I wrote a simple game based around one of their descriptions of what they wanted help with which contained an example of a [video published by Brackeys on YouTube](https://www.youtube.com/watch?v=AcRuRtBhF2U&t=286s).

## What Does It Have
Well I commented everything so people know what the code is for and so they can modify and edit it however they like. In the game the player labeled (Artifact) is controlled using the cursor (mouse) and is chaced by enemies that could do 1 of 2 actions based on their spawn, those actions are to just move towards to player to collider or move towards the player to orbit them. The artifact has 2 different ways to move, one which uses a rigidbody and one which does not, this can be selected by enabling or disabling "Move Without Cursor" in the Artifact script on the inspector. This project was created in Unity 2019.3.9f1 so please try and use that version or higher if you can. The project uses the Post Processing Stack V2 and the LWRP to add some nice looks and effects. I added a curvature like a CRT TV but didn't bother to add a scanlines effect, which if you would like to do then go ahead and do so. The LWRP (Light Weight Render Pipeline) is setup to have lighting work with 2D for a nice lighting effect on the player and enemies, this was added to show the "new" 2D lighting in Unity. I added Gizmos so people can see their adjustments in the editor's display, this was added so people learning can know what Gizmo's are for.

## Scripts
### Artifact
The Artifact script contains variables for adjustment such as "speedMultiplier", "maxVelocity", "frictionMultiplier", and "moveWithoutCursor". Move without cursor will enable a unique yet odd way of movement that uses a rigidbody.

### EnemyController
The EnemyController script has many more variables and these include "artifact" which is the Artifact script prefab, "speedMultiplier", "maxVelocity", "frictionMultiplier", "repelRange" which is for repeling other enemies away from each other, "repelForce" which is the force used to repel, "orbitArtifact" which is to determine if the enemy will orbit the Artifact or not, "orbitRange" which is the distance away from the Artifact before it repels. It also has a static list named "enemies" containing all the EnemyControllers active so it is not needed to check each update, this is good for efficiency.

### EnemySpawner
The EnemySpawner has a few avaliable adjustments such as "artifact" which should be set to the current artifact in the scene and is public to allow this to be changed out of the class, "enemyPrefab" which is for a prefab that is not in the scene but can also be set to one in the scene if really needed, "spawnOnStart" which will determine if it will spawn directly on start then timer till next or just time till next then spawn, "innerZoneRange" the zone they cannot spawn in (circle range), "outerZoneRange" the zone they can spawn in (circle range) this creates a donut shape on the spawnable range which I set to be a child to the Artifact to allow a moving spawner, this is not needed, "timeToSpawn" which is the time delay between each spawn of enemies, "minSpawn" the minimum amount of enemies to spawn, "maxSpawn" the maximum amount of enemies to spawn, "maxEnemies" the maximum amount of enemies allowed alive before the spawner stops, this can be used for an endless type of game where you have to kill enemies and it'll just spawn more and more until you lose, you can adjust this script to progressively spawn more difficult enemies, "spawnOrbiters" this determines if orbit enemies will spawn, "chanceOfOrbiter" this is a percentage change of an orbiter enemy spawning (0 = 0%, 0.5 = 50%, 1 = 100%). This script can be adjusted in many ways and as of now is very simple and is easy to learn something from.

## Post Processing
This project contains the Post Processing Stack V2 to allow for some nice simple "filters/effects" to add a touch of design. The post processing is used on the layer named "PostProcessing" and is has a volume on the camera.

## LWRP
This project contains Unity's Light Weight Render Pipeline to allow 2D lighting, although it isn't used much and is very subtle, the way it is setup someone can learn to use 2D lighting. Assets for LWRP are in the Rendering folder, the "Pipeline Asset" is set to be used in the "Graphics" tab under "Player Settings". The "Pipeline Asset" has the renderer set to be "2D RP" which is a 2D renderer generated by Unity.

## Rights
The project and code can be resused in anyway aslong as you don't sell the project as it is. You can use the code or project in your own project and publish/sell it if you'd like.
