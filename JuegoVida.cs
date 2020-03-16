/*-------------------------------------------------------------------------------------------------------------
 *  Programa:		ElJuegoDeLaVida
 *  Autor:		    Francisco J. Gómez Florido
 *  Versión:		v 1.0 nov 2018
 *  Descripción:	En este programa se encuentra la clase JuegoVida que permite crear un entorno con vida, poder guardarlo y abrirlo y un Main que prueba dichas funcionalidades.
-------------------------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ElJuegoDeLaVidaCSharp
{
    /// <summary>
    /// Clase que permite la creación de objetos JuegoVida. Te permite rellenar el entorno de celulas vivas y muertas aleatoriamente, pintar el entorno, hacer que evolucione, guardar y abrir archivos.
    /// </summary>
    public class JuegoVida
    {
        #region CAMPOS
        private const char CARACTER_PINTA_VIVA = '*';               //ES EL CARACTER QUE PINTA LAS CÉLULAS VIVAS
        private const char CARACTER_PINTA_MUERTA = ' ';             //ES EL CARACTER QUE PINTA LAS CÉLULAS MUERTAS
        private const int ALTO_MINIMO = 6;                          //ALTO MÍNIMO PERMITIDO ES IGUAL 6
        private const int ANCHO_MINIMO = 12;                        //ANCHO MÍNIMO PERMITIDO ES IGUAL 12

        private int ALTO_MAXIMO = Console.LargestWindowHeight - 2;  //ALTO MÁXIMO PERMITIDO ES IGUAL AL ALTO MÁXIMO DE PANTALLA PERMITIDO
        private int ANCHO_MAXIMO = Console.LargestWindowWidth - 1;  //ANCHO MÁXIMO PERMITIDO ES IGUAL AL ANCHO MÁXIMO DE PANTALLA PERMITIDO
        private int ancho;                                          //ANCHO DEL ENTORNO
        private int alto;                                           //ALTO DEL ENTORNO
        private bool[,] entorno;                                    //PRIMER ENTORNO, EL QUE SE MUESTRA
        private bool[,] entorno2;                                   //SEGUNDO ENTORNO, SIRVE DE APOYO PARA EVOLUCIONAR
        private ConsoleColor colorPinta;                            //ES EL COLOR CON EL QUE PINTA 
        private int stopMilisegundos;
        #endregion

        #region CONSTRUCTORES
        /// <summary>
        /// Establece el alto 30, ancho 120 y pinta las células vivas con '*' y muertas con ' '.
        /// </summary>
        public JuegoVida()
        {
            Alto = 30;
            Ancho = 120;
            Entorno = new bool[Alto, Ancho];
            ColorPinta = ConsoleColor.White;
            ContadorGeneraciones = 0;
            stopMilisegundos = 50;
        }

        /// <summary>
        /// Establece el alto y ancho indicado (min: 6 y 12, max: Console.LargestWindowHeight y Console.LargestWindowWidth) y pinta las células vivas con '*' y muertas con ' '.
        /// </summary>
        /// <param name="alto">Establece el alto del entorno donde se reproducen las células</param>
        /// <param name="ancho">Establece el ancho del entorno donde se reproducen las células</param>
        public JuegoVida(int alto, int ancho)
        {
            Alto = alto;
            Ancho = ancho;
            Entorno = new bool[Alto, Ancho];
            ColorPinta = ConsoleColor.White;
            ContadorGeneraciones = 0;
            stopMilisegundos = 50;
        }

        /// <summary>
        /// Establece el alto y ancho indicado (min: 6 y 12, max: Console.LargestWindowHeight y Console.LargestWindowWidth) y pinta las células vivas con el caracter que le indiques y muertas con ' '.
        /// </summary>
        /// <param name="alto">Establece el alto del entorno donde se reproducen las células</param>
        /// <param name="ancho">Establece el ancho del entorno donde se reproducen las células</param>
        /// <param name="pintaViva">Establece el caracter que se pinta cuando hay una célula viva</param>
        public JuegoVida(int alto, int ancho, ConsoleColor colorPintaViva)
        {
            Alto = alto;
            Ancho = ancho;
            Entorno = new bool[Alto, Ancho];
            ColorPinta = colorPintaViva;
            ContadorGeneraciones = 0;
            stopMilisegundos = 50;
        }

        /// <summary>
        /// Establece un entorno con las proporciones del array pasado (min: 6 y 12, max: Console.LargestWindowHeight y Console.LargestWindowWidth) y pinta las células vivas con '*' y muertas con ' '.
        /// </summary>
        /// <param name="entorno">bool[] que recibe con tamaño máximo Console.LargestWindowHeight y Console.LargestWindowWidth</param>
        public JuegoVida(bool[,] entorno)
        {
            Entorno = entorno;
            ColorPinta = ConsoleColor.White;
            ContadorGeneraciones = 0;
            stopMilisegundos = 50;
            Ancho = entorno.GetLength(0);
            Alto = entorno.GetLength(1);
        }

        #endregion

        #region PROPIEDADES
        /// <summary>
        /// Milisegundos que pausa la aplicación entre evoluciones.
        /// </summary>
        public int StopMilisegundos
        {
            get { return stopMilisegundos; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > int.MaxValue)
                    value = int.MaxValue;
                else
                    stopMilisegundos = value;
            }
        }

        /// <summary>
        /// Devuelve el número de generaciones que han transcurrido
        /// </summary>
        public decimal ContadorGeneraciones { get; private set; }

        /// <summary>
        /// Devuelve el StringBuilder que compone el entorno
        /// </summary>
        public StringBuilder EntornoString { get; private set; }

        /// <summary>
        /// Devuelve o establece el ancho del entorno. (mín: 12, máx: Console.LargestWindowWidth).
        /// </summary>
        public int Ancho
        {
            get { return ancho; }
            set
            {
                if (value < ANCHO_MINIMO)
                    ancho = ANCHO_MINIMO;
                else if (value > ANCHO_MAXIMO)
                    ancho = ANCHO_MAXIMO;
                else
                    ancho = value;
            }
        }

        /// <summary>
        /// Devuelve o establece el alto del entorno. (mín: 6, máx: Console.LargestWindowHeight).
        /// </summary>
        public int Alto
        {
            get { return alto; }
            set
            {
                if (value < ALTO_MINIMO)
                    alto = ALTO_MINIMO;
                else if (value > ALTO_MAXIMO)
                    alto = ALTO_MAXIMO;
                else
                    alto = value;
            }
        }

        /// <summary>
        /// Devuelve o establece el entorno donde estarán las células. (tamaño min: 6 y 12, max: Console.LargestWindowHeight y Console.LargestWindowWidth)
        /// </summary>
        public bool[,] Entorno
        {
            get { return entorno; }
            set
            {
                entorno = value;
                entorno2 = new bool[Alto, Ancho];                               //DARLE VALOR A ENTORNO2 CON LAS MISMAS PROPORCIONES QUE ENTORNO
            }
        }

        /// <summary>
        /// Devuelve o establece el color con el que se pinta el entorno.
        /// </summary>
        public ConsoleColor ColorPinta
        {
            get { return colorPinta; }
            set
            {
                if (value.CompareTo(Console.BackgroundColor) == 0)
                    throw new ArgumentException("No se puede establecer el color con el que se pinta igual que el color de fondo.");
                else
                {
                    colorPinta = value;
                    Console.ForegroundColor = ColorPinta;           //ESTABLECE EL COLOR CON EL QUE PINTA
                }
            }
        }

        #endregion

        #region MÉTODOS

        #region MÉTODOS PRIVADOS   
        /// <summary>
        /// Cuenta el número de vecinos en una posición determinada. Entorno con forma de toroide.
        /// </summary>
        /// <param name="posY">Posición respecto a la vertical</param>
        /// <param name="posX">Posición respecto a la horizontal</param>
        /// <returns>Devuelve el número de vecinos en una posición determinada</returns>
        private int ContarVecino(int posY, int posX)
        {
            int resultado = 0;                                              //RESULTADO COMIENZA EN 0
            int posRelX;                                                    //POSICION QUE TIENE EN CASO DE QUE AL REDEDOR NO HAYA CÉLULA
            int posRelY;                                                    //POSICION QUE TIENE EN CASO DE QUE AL REDEDOR NO HAYA CÉLULA
            for (int i = posY - 1; i <= (posY + 1); i++)          //ALTO      //RECORRE POSICIONES ALREDEDOR DE LA CELULA VIVA (O MUERTA) A CONTAR
            {
                for (int j = posX - 1; j <= (posX + 1); j++)      //ANCHO
                {
                    if (!(i == posY && j == posX))                             //SI NO ES LA POSICIÓN DESDE DONDE SE MIRA (CUENTA VECINOS MENOS ÉL MISMO)
                    {
                        //ALTO RELATIVO
                        if (i == -1)                                        //SI POSICIÓN NEGATIVA
                            posRelY = (posY + Alto - 1) % Alto;             //POSICIÓN RELATIVA SERÁ LA DEL EXTREMO
                        else if (i >= Alto)                                 //SI MAYOR O IGUAL AL ALTO
                            posRelY = (posY + Alto + 1) % Alto;             //POSICIÓN RELATIVA SERÁ LA DEL EXTREMO
                        else                                                //SINO POSICIÓN RELATIVA SERÁ EL VALOR DE I
                            posRelY = i;

                        //ANCHO RELATIVO
                        if (j == -1)                                        //SI POSICIÓN NEGATIVA
                            posRelX = (posX + Ancho - 1) % Ancho;           //POSICIÓN RELATIVA SERÁ LA DEL EXTREMO
                        else if (j >= Ancho)                                //SI MAYOR O IGUAL AL ANCHO
                            posRelX = (posX + Ancho + 1) % Ancho;           //POSICIÓN RELATIVA SERÁ LA DEL EXTREMO
                        else                                                //SINO POSICIÓN RELATIVA SERÁ EL VALOR DE J
                            posRelX = j;

                        if (entorno[posRelY, posRelX])                      //SI LA CÉLULA ESTÁ VIVA, LA SUMA
                            resultado++;
                    }
                }
            }
            return resultado;                                               //DEVUELVE EL NÚMERO DE VECINOS QUE TIENE
        }

        /// <summary>
        /// Recorre entorno y va almacenando las posibles celulas muertas y vivas de cada generación a entorno2.
        /// </summary>
        private void TransformaEnEntorno2()
        {
            for (int i = 0; i < Alto; i++)          //RECORRE ALTO
                for (int j = 0; j < Ancho; j++)     //RECORRE ANCHO
                {
                    int vecinosContados = ContarVecino(i, j); //CUENTA EL NÚMERO DE VECINOS Y LO ALMACENA

                    if (entorno[i, j] && vecinosContados < 2) //SI ESTÁ VIVA Y VECINOS MENOR A 2, MUERE DE SOLEDAD
                        entorno2[i, j] = false;
                    else if (entorno[i, j] && (vecinosContados == 2 || vecinosContados == 3)) //SI ESTÁ VIVA Y VECINOS SON ENTRE 2 Y 3, PERMANECE VIVA (ES REDUNDANTE)
                        entorno2[i, j] = true;
                    else if (entorno[i, j] && vecinosContados > 3)  //SI ESTÁ VIVA Y VECINOS ES MAYOR A 3 MUERE DE SUPERPOBLACIÓN
                        entorno2[i, j] = false;
                    else if (!entorno[i, j] && vecinosContados == 3) //SI ESTÁ MUERTA Y TIENE 3 VECINOS REVIVE
                        entorno2[i, j] = true;
                }
        }

        /// <summary>
        /// Recorre entorno2 y devuelve los valores transformados a entorno
        /// </summary>
        private void CopiaeAEntorno1()
        {
            for (int i = 0; i < Alto; i++)          //RECORRE ENTORNO2 Y LO COPIA A ENTORNO
                for (int j = 0; j < Ancho; j++)
                    entorno[i, j] = entorno2[i, j];

        }

        private void ObtenerContadorYEntornoString(StringBuilder archivoLeido, StringBuilder entornoString, ref decimal numeroContador)
        {
            string contadorString = "";                                             //CONTADOR EN STRING PARA LUEGO TRANSFORMAR A DECIMAL

            for (int i = 0; i < archivoLeido.Length; i++)                           //RECORRE EL TEXTO 
                                                                                    //SI ES CaracterPintaViva O CaracterPintaMuerta LO AÑADE A entornoString
                if (archivoLeido[i].Equals(CARACTER_PINTA_VIVA) || archivoLeido[i].Equals(CARACTER_PINTA_MUERTA))
                    entornoString.Append(archivoLeido[i]);
                //SI NO LO GUARDA EN contadorString
                else
                    contadorString += archivoLeido[i];

            numeroContador = decimal.Parse(contadorString);                         //TRANSFORMA contadorString A DECIMAL
        }

        private static void LeerFicheroYObtenerAnchoYAlto(String nombreFichero, ref int ancho, ref int alto, StringBuilder archivoLeido)
        {
            using (StreamReader lectorVivas = new StreamReader(nombreFichero))
            {
                string linea;
                while ((linea = lectorVivas.ReadLine()) != null)
                {                    //MIENTRAS NO SEA NULL LEE DEL ARCHIVO
                    archivoLeido.Append(linea);                                     //GUARDA EN STRING ARCHIVO TODOS LOS CARACTERES DEL ARCHIVO
                    alto++;
                }
                ancho = archivoLeido.Length / alto;
            }
        }

        private void TransformarEntornoDeStringAArrayBool(StringBuilder entornoString, bool[,] entornoArray)
        {
            int vecesI = 0;
            int vecesJ = 0;
            for (int i = 0; i < entornoString.Length; i++)                          //RECORRE ENTORNO STRING Y 
            {
                if (entornoString[i].Equals(CARACTER_PINTA_VIVA))                     //SI UN CARACTER ES IGUAL CaracterPintaViva LO PONE COMO TRUE, 
                    entornoArray[vecesI, vecesJ] = true;
                else if (entornoString[i].Equals(CARACTER_PINTA_MUERTA))              //SI ENCUENTRA CaracterPintaMuerta LO PONE COMO FALSE
                    entornoArray[vecesI, vecesJ] = false;

                //MODIFICAR VARIABLES PARA RECORRER LAS DOS DIMENSIONES DEL bool[,] leidoArchivo
                if (vecesJ == Ancho - 1)    //YA HA RECORRIDO POSICIÓN MÁXIMA DE J --> SE INCREMENTA vecesI Y SE PONE A 0 vecesJ 
                {
                    vecesI++;
                    vecesJ = 0;
                }
                else                        //SI NO HA RECORRIDO vecesJ HASTA SU POSICIÓN MÁXIMA --> INCREMENTA vecesJ
                    vecesJ++;
            }
        }
        #endregion

        #region MÉTODOS PÚBLICOS
        /// <summary>
        /// Rellena el entorno con un porcentaje de células vivas colocadas aleatoriamente.
        /// </summary>
        /// <param name="rnd">Semilla para el aleatorio</param>
        /// <param name="porcientoVida">Porcentaje de vida que queremos establecer</param>
        public void RellenaAleaVida(Random rnd, int porcientoVida)
        {
            ContadorGeneraciones = 0;
            if (porcientoVida > 100)    //AJUSTE DE MÁXIMO Y MÍNIMO DEL PORCENTAJE
            {
                porcientoVida = 100;
            }
            else if (porcientoVida < 0)
            {
                porcientoVida = 0;
            }
            for (int i = 0; i < Alto; i++)  //RECORRER EL ARRAY Y RELLENARLO CON TRUE SI EL NÚMERO ALEATORIO ESTÁ ENTRE EL PORCENTAJE INDICADO Y FALSE EN CASO CONTRARIO
            {
                for (int j = 0; j < Ancho; j++)
                {
                    int probabilidad = rnd.Next(101);       //OBTIENE NÚMERO ALEATORIO ENTRE 0-100
                    if (probabilidad <= porcientoVida)      //SI NÚMERO OBTENIDO ES MENOR O IGUAL AL PORCENTAJE INDICADO: VERDAD
                        entorno[i, j] = true;
                    else                                    //SINO: FALSO
                        entorno[i, j] = false;
                }
            }
        }

        /// <summary>
        /// Recorre el entorno y pinta por pantalla las células vivas y muertas encontradas en su interior desde la posición (0,0).
        /// </summary>
        public void PintaEntorno()
        {
            if (Ancho >= Console.WindowWidth)                 //SI LA VENTANA ES MENOR A LAS PROPORCIONES DEL ARRAY A MOSTRAR, AJUSTAR LA VENTANA
                Console.WindowWidth = Ancho + 1;
            if (Alto >= Console.WindowHeight)
                Console.WindowHeight = Alto;

            Console.SetCursorPosition(0, 0);                //POSICIONA AL PRINCIPIO
            EntornoString = new StringBuilder();
            for (int i = 0; i < Alto; i++)                  //RECORRE ENTORNO PARA GUARDARLO EN UN STRING
            {
                for (int j = 0; j < Ancho; j++)
                {
                    if (entorno[i, j])                      //SI ESTÁ VIVA ALMACENA CARACTERPINTAVIVA, SINO ALMACENA CARACTERPINTAMUERTA
                        EntornoString.Append(CARACTER_PINTA_VIVA);
                    else
                        EntornoString.Append(CARACTER_PINTA_MUERTA);
                }
                EntornoString.Append("\n");                      //CADA FILA ALMACENA SALTO DE LINEA
            }
            Console.WriteLine(EntornoString.ToString());               //ESCRIBE POR PANTALLA EL STRING ALMACENADO
        }

        /// <summary>
        /// Método que hace evolucionar al entorno y lo muestra por pantalla desde la posición (0,0).
        /// </summary>
        public void Evoluciona()
        {
            TransformaEnEntorno2();     //REALIZA CAMBIOS EN EL ENTORNO 2
            CopiaeAEntorno1();          //DEVUELVE LOS CAMBIOS AL ENTORNO
            PintaEntorno();             //LO MUESTRA POR PANTALLA
            if (ContadorGeneraciones < decimal.MaxValue)    //SI NO ES EL MÁXIMO DE UN DECIMAL
                ContadorGeneraciones++;                     //Y AUMENTA EL NÚMERO DE GENERACIONES

            Thread.Sleep(StopMilisegundos);                 //PAUSA EL THREAD PARA PODER OBSERVAR LA EVOLUCIÓN REALIZADA
        }

        /// <summary>
        /// Permite recuperar mediante un archivo .txt el estado de un entorno almacenado anteriormente.
        /// </summary>
        public void AbrirArchivo()
        {
            OpenFileDialog dialogoApertura = new OpenFileDialog();  //CREAR OPENFILEDIALOG PARA SELECCIONAR QUE ARCHIVO ABRIR
            dialogoApertura.DefaultExt = "cons";
            dialogoApertura.Filter = "Archivo juego de la vida (*.cons)|*.cons";
            if (dialogoApertura.ShowDialog() == DialogResult.OK)    //SI SE SELECCIONA UN ARCHIVO
            {
                ContadorGeneraciones = 0;                                               //ESTABLECER CONTADOR A 0
                int alto = 0;
                int ancho = 0;
                StringBuilder archivoLeido = new StringBuilder();
                //LEER LINEAS DEL ARCHIVO Y CERRARLO (NO INCLUYE LOS SALTOS DE LÍNEA)
                LeerFicheroYObtenerAnchoYAlto(dialogoApertura.FileName, ref ancho, ref alto, archivoLeido);
                Alto = alto;
                Ancho = ancho;

                StringBuilder entornoString = new StringBuilder();
                decimal numeroContador = 0;

                ObtenerContadorYEntornoString(archivoLeido, entornoString, ref numeroContador);
                ContadorGeneraciones = numeroContador;                                  //ALMACENA EN contadorGeneraciones EL NÚMERO DEL CONTADOR OBTENIDO

                //RELLENAR ARRAY MULTIDIMENSIONAL DESDE STRING (ARRAY DE UNA DIMENSION DE CHAR)
                bool[,] entornoArray = new bool[Alto, Ancho];
                TransformarEntornoDeStringAArrayBool(entornoString, entornoArray);
                Entorno = entornoArray;

                //MENSAJE INDICANDO QUE EL ARCHIVO HA SIDO CARGADO
                Console.SetCursorPosition(20, 22);
                Console.WriteLine("Archivo cargado correctamente. Pulse cualquier tecla para continuar...");

                while (!Console.KeyAvailable)           //MIENTRAS NO SE PULSE UNA TECLA BUCLE INFINITO
                { }

                Console.SetCursorPosition(20, 22);      //BORRA MENSAJE ANTES ESCRITO
                Console.WriteLine("                                                                                ");
                PintaEntorno();             //PINTA EL ARCHIVO CARGADO
            }
        }

        /// <summary>
        /// Permite almacenar mediante un cuadro de diálogo un archivo .txt con el estado actual del entorno y el contador de generaciones.
        /// </summary>
        public void GuardarArchivo()
        {
            SaveFileDialog dialogoGuardar = new SaveFileDialog();       //CREAR SAVEFILEDIALOG PARA SELECCIONAR DONDE GUARDAR EL ARCHIVO
            dialogoGuardar.DefaultExt = "cons";                          //FORMATO POR DEFECTO DEL ARCHIVO .cons
            dialogoGuardar.Filter = "Archivo juego de la vida (*.cons)|*.cons";

            if (dialogoGuardar.ShowDialog() == DialogResult.OK)         //SI LE DAS A GUARDAR
            {
                StreamWriter guarda = new StreamWriter(dialogoGuardar.FileName);    //CREA STREAM PARA ESCRIBIR EN UN ARCHIVO QUE SE VA A CREAR O SOBREESCRIBIR
                guarda.Write(ContadorGeneraciones + EntornoString.ToString());                 //ESCRIBE ContadorGeneracionesS Y EntornoString
                guarda.Close();                                                     //CIERRA EL FLUJO DEL STREAM ABIERTO
            }
        }
        #endregion

        #endregion
    }
}