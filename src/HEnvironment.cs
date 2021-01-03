using System.Collections.Generic;
using UnityEngine;
namespace HermeticaInterpreter{
    public class HEnvironment : Environment{
        
        public Hermetica.Game game;
        public HEnvironment() {
            enclosing = null;
        }

        public HEnvironment(HEnvironment newEnclosing) {
            this.enclosing = newEnclosing;
        }
        public void SetGame(Hermetica.Game game){
            Debug.Log("Setup interpreter...");
            this.game = game;
            define("local",game.player1.name);
            define("enemy",game.player2.name);
            defineModel("GAME", game);
            defineModel("PLAYER", game.player1);
            defineModel("ENEMY", game.player1);
        }

        

    }
}
