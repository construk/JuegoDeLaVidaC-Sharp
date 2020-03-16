using System;

namespace ElJuegoDeLaVidaCSharp
{
    class JugandoMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.CursorVisible = false;                              //OCULTAR CURSOR
                Random rnd = new Random();                                  //CREAR SEMILLA PARA ALEATORIO
                ConsoleKeyInfo teclaPulsada = new ConsoleKeyInfo();         //TACLA A PULSAR
                Console.OutputEncoding = System.Text.Encoding.UTF8;         //CAMBIAR CODIFICACIÓN PARA ACEPTAR OTROS CARÁCTERES
                JuegoVida juegoVida = new JuegoVida(22, 120);               //CREO JUEGO DE LA VIDA DE 22 DE ALTO Y 120 DE ANCHO
                juegoVida.RellenaAleaVida(rnd, 20);                         //RELLENA ALEATORIAMENTE EL ENTORNO CON 20% DE VIDA
                juegoVida.StopMilisegundos = 200;                           //ESTABLECE COMO TIEMPO DE PARADA POR DEFECTO 200 MILISEGUNDOS
                juegoVida.PintaEntorno();                                   //PINTA EL ENTORNO
                EscribeOpciones();                                          //PINTA LAS OPCIONES

                do                                                          //HACER MIENTRAS NO SE PULSE LA TECLA ESCAPE
                {
                    EscribeContador(25, 28, juegoVida);                     //PINTA EL CONTADOR DE LAS GENERACIONES 
                    teclaPulsada = Console.ReadKey(true);                   //LEE CARACTER
                    switch (teclaPulsada.Key)                               //SEGÚN EL CARACTER PULSADO (AUTOMATICO, ITERA, REINICIA, GUARDA O CARGA (ESCAPE PARA SALIR) 
                    {
                        case ConsoleKey.A:                                  //SI LA TECLA PULSADA ES LA A, O LA FLECHA HACIA ARRIBA O LA FLECHA HACIA ABAJO
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                            while (!Console.KeyAvailable)                   //MIENTRAS NO SE PULSE UNA TECLA
                            {
                                juegoVida.Evoluciona();                     //VA A EVOLUCIONAR 
                                //SI HAS PULSADO LA TECLA HACIA ARRIBA --> INCREMENTAS VELOCIDAD (QUITANDO STOP DE MILISEGUNDOS)
                                if (teclaPulsada.Key == ConsoleKey.UpArrow)
                                    juegoVida.StopMilisegundos -= 20;
                                //SI HAS PULSADO LA TECLA HACIA ABAJO --> DISMINUYES VELOCIDAD (AUMENTANDO STOP DE MILISEGUNDOS)
                                else if (teclaPulsada.Key == ConsoleKey.DownArrow)
                                    juegoVida.StopMilisegundos += 20;

                                EscribeContador(25, 28, juegoVida);         //PINTAR EL CONTADOR
                                teclaPulsada = new ConsoleKeyInfo();        //BORRA LA TECLA PULSADA (INICIALIZÁNDOLA DE NUEVO)
                            }
                            break;
                        case ConsoleKey.I:                                  //SI PULSAS I
                            juegoVida.Evoluciona();                         //EVOLUCIONA UNA VEZ
                            break;
                        case ConsoleKey.R:                                  //SI PULSAS R
                            juegoVida.RellenaAleaVida(rnd, 20);             //RELLENA ALEATORIAMENTE
                            juegoVida.PintaEntorno();                       //PINTA ENTORNO
                            BorrarContador();
                            break;
                        case ConsoleKey.S:                                  //SI PULSAS S
                            juegoVida.GuardarArchivo();                     //EJECUTA MÉTODO PARA GUARDAR EL ESTADO
                            break;
                        case ConsoleKey.L:                                  //SI PULSAS L
                            juegoVida.AbrirArchivo();                       //EJECUTA MÉTODO PARA ABRIR ARCHIVO DE ESTADO GUARDADO
                            BorrarContador();
                            break;
                    }
                } while (teclaPulsada.Key != ConsoleKey.Escape);            //MIENTRAS NO SE PULSE ESCAPE....
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); Console.ReadLine(); }           //SI EXCEPCIÓN --> MENSAJE Y ESPERA PARA VERLO
        }
        #region MÉTODOS PRIVADOS
        private static void BorrarContador()
        {
            Console.SetCursorPosition(20, 28);              //BORRAR EL CONTADOR
            Console.Write("                                                                                ");
        }

        /// <summary>
        /// Escribe el contador de número de generaciones del juego de la vida en una posición determinada
        /// </summary>
        /// <param name="posX">Posición horizontal</param>
        /// <param name="posY">Posición vertical</param>
        /// <param name="medio">Juego al que aplicar</param>
        private static void EscribeContador(int posX, int posY, JuegoVida medio)
        {
            Console.SetCursorPosition(posX, posY);
            if (medio.ContadorGeneraciones <= decimal.MaxValue)
                Console.WriteLine("\t\tContador de generaciones: {0}\t Stop en milisegundos: {1:D6}", medio.ContadorGeneraciones, medio.StopMilisegundos);
            else
                Console.WriteLine("\t\t\tContador de generaciones: VALOR MÁXIMO ALCANZADO");
        }

        /// <summary>
        /// Escribe las opciones del menú en una posición fija
        /// </summary>
        private static void EscribeOpciones()
        {
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("  I --> Iterar");
            Console.WriteLine("  A --> Automático\t↑Aumenta velocidad\t↓Disminuye la velocidad");
            Console.WriteLine("  R --> Reiniciar");
            Console.WriteLine("  S --> Guardar");
            Console.WriteLine("  L --> Cargar");
            Console.WriteLine("  ESC --> Salir");
        }
        #endregion
    }
}