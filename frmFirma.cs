using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryEliasEtapa1
{
    public partial class frmFirma : Form
    {

        //Declaro una variable tipo Bitmap
        private Bitmap firmaBitmap;
        public frmFirma()
        {
            InitializeComponent();

            //Creo un nuevo bitmap del mismo tamaño que el PictureBox
            firmaBitmap = new Bitmap(pbFirma.Width, pbFirma.Height);

            //Asigno el bitmap al PictureBox
            pbFirma.Image = firmaBitmap;
        }

        //Creo una variable booleana para darle valor true cuando el usuario este dibujando sobre el PictureBox 
        private bool dibujando = false;

        //La variable de tipo Point es una estructura que representa un par de coordenadas x e y 

        //Creo una variable de tipo point para almacenar la posición anterior del cursor del mouse mientras el usuario 
        //dibuja
        private Point posicionAnterior;

        private void pbFirma_MouseDown(object sender, MouseEventArgs e)
        {
            //Al poner la variable en true indico que el usuario esta dibujando sobre el PictureBox
            dibujando = true;

            //Se actualiza con la posición del mouse en el momento que se hace click
            posicionAnterior = e.Location;
        }

        private void pbFirma_MouseMove(object sender, MouseEventArgs e)
        {
            //Si está dibujando el usuario ingresa al if 
            if (dibujando == true)
            {
                //Con el objeto Graphics del PictureBox dibuja una línea entre la posición anterior y la actual del cursor
                using (Graphics lapiz = Graphics.FromImage(firmaBitmap))
                {
                    //Dibujo una línea desde la posición anterior a la posición actual
                    lapiz.DrawLine(Pens.Black, posicionAnterior, e.Location);

                    //Actualizo la posición anterior para el próximo movimiento
                    posicionAnterior = e.Location;

                    //Invalido el área del PictureBox para forzar el redibujado
                    pbFirma.Invalidate();
                }
            }
        }

        private void pbFirma_MouseUp(object sender, MouseEventArgs e)
        {
            dibujando = false;
        }

        private void LimpiarPictureBox()
        {
            using (Graphics lapiz = Graphics.FromImage(pbFirma.Image))
            {
                //Dibujo un rectángulo blanco del tamaño del PictureBox para limpiar la imagen
                lapiz.FillRectangle(Brushes.White, 0, 0, pbFirma.Width, pbFirma.Height);
            }

            //Fuerzo el redibujado del PictureBox para reflejar los cambios
            pbFirma.Invalidate();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (firmaBitmap != null)
            {
                try
                {
                    //Obtengo la ruta de la carpeta "Imagenes Firmas" en la solución (Se crea en Bin - Debug - net6.0-windows)
                    string carpetaImagenesFirmas = Path.Combine(Application.StartupPath, "Imágenes Firmas");

                    //Creo la carpeta si no existe
                    if (!Directory.Exists(carpetaImagenesFirmas))
                    {
                        Directory.CreateDirectory(carpetaImagenesFirmas);
                    }

                    //Guardo la firma en la carpeta "Imagenes Firmas" con un nombre único
                    string nombreArchivo = txtNombreFirma.Text + $"_firma_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";

                    //Combino las rutas para almacenarlas en una sola y utilizarla para guardar la imagen
                    string rutaArchivo = Path.Combine(carpetaImagenesFirmas, nombreArchivo);

                    //Con el método Save guardo la imagen en un archivo en el disco y le indico que va a ser formato Png
                    firmaBitmap.Save(rutaArchivo, System.Drawing.Imaging.ImageFormat.Png);

                    MessageBox.Show("Firma guardada correctamente en la carpeta Imágenes Firmas.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la firma: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No hay firma para guardar.");
            }

            LimpiarPictureBox();

            txtNombreFirma.Clear();
        }

        private void txtNombreFirma_TextChanged(object sender, EventArgs e)
        {
            //Activo y desactivo el botón Guardar dependiendo del contenido el txt
            if (txtNombreFirma.Text != "")
            {
                btnGuardar.Enabled = true;
            }
            else
            {
                btnGuardar.Enabled = false;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarPictureBox();

            txtNombreFirma.Clear();
        }
    }
}
