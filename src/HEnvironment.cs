using System.Collections.Generic;

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
            this.game = game;
            define("local",game.player1.name);
            define("enemy",game.player2.name);
            define("current_player",game.player2);
        }

        

    }
}
