## El juego de la vida

El juego de la vida es un autómata celular diseñado por el matemático británico John Horton Conway en 1970.

Consiste en un tablero dividido en una serie de  celdillas  que  pueden tener dos  estados:  VIVAS  o  MUERTAS.   
 
------------------------------------

#### Funcionamiento
Una  celda  viva  se representa con una ficha, y una muerta con la ausencia de tal ficha. En cada generación (turno), nacen unas  y  mueren  otras  según  las  siguientes reglas:   
1. Si una celdilla está  viva y tiene 2 o 3 vecinas permanece viva. 
2. Si una celdilla está  viva y  tiene  menos  de  2  vecinas muere de soledad. 
3. Si una celdilla está   viva  y  tiene  m s  de  3  vecinas muere por sobrecarga de población. 
4. Si una celdilla está  muerta y tiene exactamente 3 vecinas entonces pasa a estar viva (nace). 

![Imagen funcionamiento juego vida](https://github.com/construk/JuegoDeLaVidaC-Sharp/JuegoVidaExample.gif)

------------------------------------

#### Funcionalidad
* Pulsar A para iteración de forma automática.
* Pulsar I para iterar una vez.
* Pulsa las flechas hacia arriba/abajo se incrementa/disminuye la velocidad de ejecución.
* Pulsa R para reiniciar el tablero.
* Pulsar S para guardar el estado actual del tablero.
* Pulsar L para cargar un fichero al tablero.
* ESC para salir de la aplicación.