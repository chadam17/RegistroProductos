using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;


namespace Registro
{
    
    public partial class R : Form
    {
        private int index;
        private int indexColumna;
        private string valor;
        private FormAdd fa = new FormAdd();
        private FormMod fm = new FormMod();
        private FormCheck fch = new FormCheck();
        private DataTable dt = new DataTable();
        public R()
        {
            InitializeComponent();
            btnMod.Enabled = false;
            btnBorrar.Enabled = false;
            btnCheck.Enabled = false;
            btnExp.Enabled = false;
            // Check which column is selected, otherwise set NewColumn to null.
            DataGridViewColumn newColumn =
                dataGridView1.Columns.GetColumnCount(
                DataGridViewElementStates.Selected) == 1 ?
                dataGridView1.SelectedColumns[0] : null;

            DataGridViewColumn oldColumn = dataGridView1.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    dataGridView1.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // If no column has been selected, display an error dialog  box.
            if (newColumn != null)
            {
                dataGridView1.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Ascending ?
                    SortOrder.Ascending : SortOrder.Descending;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormAdd fa = new FormAdd();
            fa.ShowDialog();
            try
            { 
                string[] fila = new string[6];
                fila[0] = fa.textBox1.Text;
                fila[1] = fa.textBox2.Text;
                fila[2] = fa.textBox3.Text;
                fila[3] = fa.textBox4.Text;
                fila[4] = fa.textBox5.Text;
                fila[5] = fa.textBox6.Text;
                if ((fila[0] == "") || (fila[1] == "") || (fila[2] == "") || (fila[3] == "") || (fila[4] == "") || (fila[5] == ""))
                {
                    DialogResult dr = MessageBox.Show("Debes rellenar todos los campos");
                    fa.ShowDialog();
                }
                if ((fila[0] != "") && (fila[1] != "") && (fila[2] != "") && (fila[3] != "") && (fila[4] != "") && (fila[5] != ""))
                {
                    btnMod.Enabled = true;
                    btnBorrar.Enabled = true;
                    btnCheck.Enabled = true;
                    btnExp.Enabled = true;
                    if (dataGridView1.RowCount > 0)
                    {
                        bool existe = false;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[1].Value.ToString() == fila[1])
                            {
                                MessageBox.Show("Código ya asignado a otro producto");
                                existe = true;
                                break;
                            }
                        }
                        if (existe == false)
                            dataGridView1.Rows.Add(fila);
                    }
                    else
                    {
                        dataGridView1.Rows.Add(fila);
                        DialogResult dr = MessageBox.Show("Producto registrado");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }

            /*
            index = dataGridView1.Rows.Add();

            dataGridView1.Rows[index].Cells[0].Value = fa.textBox1.Text;
            dataGridView1.Rows[index].Cells[1].Value = fa.textBox2.Text;
            dataGridView1.Rows[index].Cells[2].Value = fa.textBox3.Text;
            dataGridView1.Rows[index].Cells[3].Value = fa.textBox4.Text;
            dataGridView1.Rows[index].Cells[4].Value = fa.textBox5.Text;
            dataGridView1.Rows[index].Cells[5].Value = fa.textBox6.Text;

            if ((fa.textBox1.Text != "") && (fa.textBox2.Text != "") && (fa.textBox3.Text != "") && (fa.textBox4.Text != "") && (fa.textBox5.Text != "") && (fa.textBox6.Text != ""))
            {
                DialogResult dr = MessageBox.Show("Producto registrado");

            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("La aplicación se cerrará. ¿Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
                Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                index = dataGridView1.CurrentRow.Index;
                if (index >= dataGridView1.RowCount)
                    index = 0;
                fa.textBox1.Text = dataGridView1[0, index].Value.ToString();
                fa.textBox2.Text = dataGridView1[1, index].Value.ToString();
                fa.textBox3.Text = dataGridView1[2, index].Value.ToString();
                fa.textBox4.Text = dataGridView1[3, index].Value.ToString();
                fa.textBox5.Text = dataGridView1[4, index].Value.ToString();
                fa.textBox6.Text = dataGridView1[5, index].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }

        

        private void btnMod_Click(object sender, EventArgs e)
        {
            try
            {
                string cod = "";
                cod = Microsoft.VisualBasic.Interaction.InputBox("Ingresa el número de fila del producto a editar");
                int i = 0, x = 0;
                for (i = 0; i <= dataGridView1.RowCount; i++)
                {
                    while (Int32.Parse(cod) == 0)
                        cod = Microsoft.VisualBasic.Interaction.InputBox("Ingresa el número de fila del producto a editar");
                    if (i == Int32.Parse(cod))
                    {
                        fm.textBox1.Text = dataGridView1.Rows[i-1].Cells[0].Value.ToString();
                        fm.textBox2.Text = dataGridView1.Rows[i-1].Cells[1].Value.ToString();
                        fm.textBox3.Text = dataGridView1.Rows[i-1].Cells[2].Value.ToString();
                        fm.textBox4.Text = dataGridView1.Rows[i-1].Cells[3].Value.ToString();
                        fm.textBox5.Text = dataGridView1.Rows[i-1].Cells[4].Value.ToString();
                        fm.textBox6.Text = dataGridView1.Rows[i-1].Cells[5].Value.ToString();
                        fm.ShowDialog();
                    }
                }
                string[] fila = new string[6];
                fila[0] = fm.textBox1.Text;
                fila[1] = fm.textBox2.Text;
                fila[2] = fm.textBox3.Text;
                fila[3] = fm.textBox4.Text;
                fila[4] = fm.textBox5.Text;
                fila[5] = fm.textBox6.Text;
                if ((fila[0] == "") || (fila[1] == "") || (fila[2] == "") || (fila[3] == "") || (fila[4] == "") || (fila[5] == ""))
                {
                    DialogResult dr = MessageBox.Show("Debes rellenar todos los campos");
                    fm.ShowDialog();
                }
                if ((fila[0] != "") && (fila[1] != "") && (fila[2] != "") && (fila[3] != "") && (fila[4] != "") && (fila[5] != ""))
                {
                    if (dataGridView1.RowCount > 0)
                    {
                        bool existe = false;
                        for (x = 0; x < dataGridView1.RowCount; x++)
                        {
                            if (dataGridView1.Rows[x].Cells[1].Value.ToString() == fila[1])
                            {
                                MessageBox.Show("Código ya asignado a otro producto");
                                existe = true;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[0].Value = fila[0];
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[1].Value = fila[1];
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[2].Value = fila[2];
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[3].Value = fila[3];
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[4].Value = fila[4];
                            dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[5].Value = fila[5];
                        }
                    }
                    else
                    {
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[0].Value = fila[0];
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[1].Value = fila[1];
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[2].Value = fila[2];
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[3].Value = fila[3];
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[4].Value = fila[4];
                        dataGridView1.Rows[Int32.Parse(cod) - 1].Cells[5].Value = fila[5];
                        DialogResult dr = MessageBox.Show("Producto registrado");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                index = dataGridView1.CurrentRow.Index;
                fch.label1.Text = "Nombre: " + dataGridView1[0, index].Value.ToString() + "\nCódigo: " + dataGridView1[1, index].Value.ToString() + "\nCantidad: " + dataGridView1[2, index].Value.ToString() + "\nPrecio: " + dataGridView1[3, index].Value.ToString() + "\nTipo: " + dataGridView1[4, index].Value.ToString() + "\nDescripción: " + dataGridView1[5, index].Value.ToString();
                fch.ShowDialog();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("No has seleccionado ningún producto");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
                indexColumna = e.ColumnIndex;
            
                switch (indexColumna)
                {
                    case 0:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre: ", "Modificar nombre");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 1:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nuevo código: ", "Modificar código");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 2:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nueva cantidad: ", "Modificar cantidad");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 3:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nuevo precio: ", "Modificar precio");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 4:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nuevo tipo: ", "Modificar tipo");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 5:
                                    valor = Microsoft.VisualBasic.Interaction.InputBox("Nueva descripción: ", "Modificar descripción");
                                    dataGridView1.CurrentCell.Value = valor;
                                    break;
                    case 6:
                                    string FileName = null;

                                    OpenFileDialog openFileDialog = new OpenFileDialog();
                                    openFileDialog.RestoreDirectory = true;

                                    openFileDialog.Filter = "All picture files (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";

                                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        FileName = openFileDialog.FileName;
                                        //((DataGridViewImageCell)dataGridView1.Rows[e.RowIndex].Cells[1]).Value = Image.FromFile(FileName);
                                        dataGridView1.CurrentCell.Value = Image.FromFile(FileName);
                                        Column7.ImageLayout = DataGridViewImageCellLayout.Stretch;
                                    }
                                    break;
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Registros exportados correctamente", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay productos registrados", "Info");
            }
        }

        private void btnImp_Click(object sender, EventArgs e)
        {        
            try
            {              
                DataTable dt = new DataTable();
                OpenFileDialog ofd = new OpenFileDialog() { Filter = "Archivo CSV|*.csv" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string sep = ";";

                    string[] lineas = File.ReadAllLines(ofd.FileName);
                    string[] cabeceras = lineas[0].Split(new[] { sep }, StringSplitOptions.None);

                    // dataGridView1.Columns.Clear();
                    foreach (string c in cabeceras)
                        dataGridView1.Columns.Add(c,c);

                    for (int i = 1; i < lineas.Length; i++)
                    {
                        string[] celdas = lineas[i].Split(new[] { sep }, StringSplitOptions.None);
                        dataGridView1.Rows.Add(celdas);
                    }
                }
                    
                btnMod.Enabled = true;
                btnBorrar.Enabled = true;
                btnCheck.Enabled = true;
                btnExp.Enabled = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (index != -1)
                {
                    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Para modificar el contenido de una celda haz doble clic sobre ella. Para editar una línea presiona el botón de Modificar. Para ordenar en función del criterio, clica sobre el nombre del campo de interés y aparecerá la flecha con el sentido ascendente o descendente de la reordenación","Tip de modificación de datos");
        }
    }
}
