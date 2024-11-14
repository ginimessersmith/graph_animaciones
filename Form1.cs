using Graphic3D.Models;
using Graphic3D.Utils;
using OpenTK;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graphic3D
{
    partial class Form1 : Form
    {
        private GLControl glControl;
        private Button open;
        private Button save;
        private TreeView sceneTree;

        public Form1()
        {
            InitializeComponent();
            InitializeGLControl();
            
        }

        


        private void open_Click(object sender, EventArgs e)
        {
            string filePath = null;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            Console.WriteLine(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    if (glControl is CustomGLControl customGLCtrl)
                    {
                        try
                        {
                            scene = JsonHelper.LoadObjectFromJsonFile<Scene>(filePath);
                            customGLCtrl.LoadScene(scene);
                            scene.Translate();
                            glControl.Invalidate();
                            DisplayTreeNode(scene);
                            Console.WriteLine("JSON data loaded successfully.");
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading JSON: {ex.Message}");
                        } 
                    }
                    else
                    {
                        Console.WriteLine("glControl is not of type CustomGLControl.");
                    }

                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading JSON: {ex.Message}");
                }
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            string filePath = null;

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON files (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }
            }
            Console.WriteLine(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    JsonHelper.SaveObjectToJsonFile<Scene>(scene, filePath);
                    Console.WriteLine("JSON data saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading JSON: {ex.Message}");
                }
                
            }
        }

       

        private void DisplayTreeNode(Scene sceneData)
        {
            sceneTree.Nodes.Clear();
            var sceneNode = new TreeNode("Escenario");
            sceneNode.Tag = sceneData;
            sceneTree.Nodes.Add(sceneNode);
            foreach (var obj in sceneData.Objects)
            {
                var objectNode = new TreeNode(obj.Key);
                objectNode.Tag = obj.Value;
                sceneNode.Nodes.Add(objectNode);
                foreach (var part in obj.Value.Parts)
                {
                    var partNode = new TreeNode(part.Key);
                    partNode.Tag = part.Value;
                    objectNode.Nodes.Add(partNode);  
                    
                }
            }
        }

        private void sceneTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = sceneTree.SelectedNode;
            label1.Text = "Nodo seleccionado: " + selectedNode.Text;
        }

        private NumericUpDown scaleX;
        private NumericUpDown rotY;
        private NumericUpDown rotX;
        private NumericUpDown translZ;
        private NumericUpDown translY;
        private NumericUpDown translX;
        private NumericUpDown scaleZ;
        private NumericUpDown scaleY;
        private NumericUpDown rotZ;
        private Label labelScale;
        private Label labelTrasl;
        private Label labelRot;
        private Scene scene;
        private Label label1;
        private Button resetScale;
        private Button resetTransl;
        private Button resetRot;

        private void scale_ValueChanged(object sender, EventArgs e)
        {
            TreeNode selected = sceneTree.SelectedNode;
            if (selected != null)
            {
                if (selected.Tag is Scene scene)
                {
                    scene.Scale((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
                }
                else if (selected.Tag is IObject selectedObject)
                {
                    selectedObject.Scale((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
                }
                else if (selected.Tag is Part selectedPart)
                {
                    selectedPart.Scale((float)scaleX.Value, (float)scaleY.Value, (float)scaleZ.Value);
                }
                else
                {
                    Console.WriteLine("Unsupported tag type.");
                }

                glControl.Invalidate();
            }
        }


        private void translation_ValueChanged(object sender, EventArgs e)
        {
            TreeNode selected = sceneTree.SelectedNode;
            if (selected != null)
            {
                if (selected.Tag is Scene scene)
                {
                    scene.Translate((float)translX.Value, (float)translY.Value, (float)translZ.Value);
                }
                else if (selected.Tag is IObject selectedObject)
                {
                    selectedObject.Translate((float)translX.Value, (float)translY.Value, (float)translZ.Value);
                }
                else if (selected.Tag is Part selectedPart)
                {
                    selectedPart.Translate((float)translX.Value, (float)translY.Value, (float)translZ.Value);
                }
                else
                {
                    Console.WriteLine("Unsupported tag type.");
                }

                glControl.Invalidate();
            }
        }

        

        private void rotation_ValueChanged(object sender, EventArgs e)
        {
            TreeNode selected = sceneTree.SelectedNode;
            if (selected != null)
            {
                if (selected.Tag is Scene scene)
                {
                    scene.Rotate(scene.Center,(float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
                }
                else if (selected.Tag is IObject selectedObject)
                {
                    selectedObject.Rotate(selectedObject.Center,(float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
                }
                else if (selected.Tag is Part selectedPart)
                {
                    selectedPart.Rotate(selectedPart.Center, (float)rotX.Value, (float)rotY.Value, (float)rotZ.Value);
                }
                else
                {
                    Console.WriteLine("Unsupported tag type.");
                }

                glControl.Invalidate();
            }
            
        }

        

        private void sceneTree_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("leave..." + sceneTree.SelectedNode);
            if (sceneTree.SelectedNode != null)
            {
                sceneTree.HideSelection = false;
            }
        }

        private void InitializeGLControl()
        {
            glControl = new CustomGLControl();
            glControl.Dock = DockStyle.Fill;

            //this.SuspendLayout();

            //this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl.Name = "glControl";
            //this.glControl1.Size = new System.Drawing.Size(225, 208);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;

            this.Controls.Add(glControl);


        }

        private void InitializeComponent()
        {
            this.open = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.sceneTree = new System.Windows.Forms.TreeView();
            this.scaleX = new System.Windows.Forms.NumericUpDown();
            this.rotY = new System.Windows.Forms.NumericUpDown();
            this.rotX = new System.Windows.Forms.NumericUpDown();
            this.translZ = new System.Windows.Forms.NumericUpDown();
            this.translY = new System.Windows.Forms.NumericUpDown();
            this.translX = new System.Windows.Forms.NumericUpDown();
            this.scaleZ = new System.Windows.Forms.NumericUpDown();
            this.scaleY = new System.Windows.Forms.NumericUpDown();
            this.rotZ = new System.Windows.Forms.NumericUpDown();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelTrasl = new System.Windows.Forms.Label();
            this.labelRot = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resetScale = new System.Windows.Forms.Button();
            this.resetTransl = new System.Windows.Forms.Button();
            this.resetRot = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.translZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.translY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.translX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).BeginInit();
            this.SuspendLayout();
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(12, 12);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(75, 36);
            this.open.TabIndex = 0;
            this.open.Text = "Open";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(123, 12);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 36);
            this.save.TabIndex = 1;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // sceneTree
            // 
            this.sceneTree.Location = new System.Drawing.Point(12, 54);
            this.sceneTree.Name = "sceneTree";
            this.sceneTree.Size = new System.Drawing.Size(186, 200);
            this.sceneTree.TabIndex = 2;
            this.sceneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sceneTree_AfterSelect);
            this.sceneTree.Leave += new System.EventHandler(this.sceneTree_Leave);
            // 
            // scaleX
            // 
            this.scaleX.DecimalPlaces = 1;
            this.scaleX.Location = new System.Drawing.Point(12, 358);
            this.scaleX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.scaleX.Name = "scaleX";
            this.scaleX.Size = new System.Drawing.Size(58, 28);
            this.scaleX.TabIndex = 4;
            this.scaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleX.ValueChanged += new System.EventHandler(this.scale_ValueChanged);
            // 
            // rotY
            // 
            this.rotY.DecimalPlaces = 1;
            this.rotY.Location = new System.Drawing.Point(76, 498);
            this.rotY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotY.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotY.Name = "rotY";
            this.rotY.Size = new System.Drawing.Size(58, 28);
            this.rotY.TabIndex = 5;
            this.rotY.ValueChanged += new System.EventHandler(this.rotation_ValueChanged);
            // 
            // rotX
            // 
            this.rotX.DecimalPlaces = 1;
            this.rotX.Location = new System.Drawing.Point(12, 498);
            this.rotX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotX.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotX.Name = "rotX";
            this.rotX.Size = new System.Drawing.Size(58, 28);
            this.rotX.TabIndex = 6;
            this.rotX.ValueChanged += new System.EventHandler(this.rotation_ValueChanged);
            // 
            // translZ
            // 
            this.translZ.DecimalPlaces = 1;
            this.translZ.Location = new System.Drawing.Point(140, 428);
            this.translZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.translZ.Name = "translZ";
            this.translZ.Size = new System.Drawing.Size(58, 28);
            this.translZ.TabIndex = 7;
            this.translZ.ValueChanged += new System.EventHandler(this.translation_ValueChanged);
            // 
            // translY
            // 
            this.translY.DecimalPlaces = 1;
            this.translY.Location = new System.Drawing.Point(76, 428);
            this.translY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.translY.Name = "translY";
            this.translY.Size = new System.Drawing.Size(58, 28);
            this.translY.TabIndex = 8;
            this.translY.ValueChanged += new System.EventHandler(this.translation_ValueChanged);
            // 
            // translX
            // 
            this.translX.DecimalPlaces = 1;
            this.translX.Location = new System.Drawing.Point(12, 428);
            this.translX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.translX.Name = "translX";
            this.translX.Size = new System.Drawing.Size(58, 28);
            this.translX.TabIndex = 9;
            this.translX.ValueChanged += new System.EventHandler(this.translation_ValueChanged);
            // 
            // scaleZ
            // 
            this.scaleZ.DecimalPlaces = 1;
            this.scaleZ.Location = new System.Drawing.Point(140, 358);
            this.scaleZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.scaleZ.Name = "scaleZ";
            this.scaleZ.Size = new System.Drawing.Size(58, 28);
            this.scaleZ.TabIndex = 10;
            this.scaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleZ.ValueChanged += new System.EventHandler(this.scale_ValueChanged);
            // 
            // scaleY
            // 
            this.scaleY.DecimalPlaces = 1;
            this.scaleY.Location = new System.Drawing.Point(76, 358);
            this.scaleY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.scaleY.Name = "scaleY";
            this.scaleY.Size = new System.Drawing.Size(58, 28);
            this.scaleY.TabIndex = 11;
            this.scaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleY.ValueChanged += new System.EventHandler(this.scale_ValueChanged);
            // 
            // rotZ
            // 
            this.rotZ.DecimalPlaces = 1;
            this.rotZ.Location = new System.Drawing.Point(140, 498);
            this.rotZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotZ.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotZ.Name = "rotZ";
            this.rotZ.Size = new System.Drawing.Size(58, 28);
            this.rotZ.TabIndex = 12;
            this.rotZ.ValueChanged += new System.EventHandler(this.rotation_ValueChanged);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(12, 334);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(53, 18);
            this.labelScale.TabIndex = 13;
            this.labelScale.Text = "Scale";
            // 
            // labelTrasl
            // 
            this.labelTrasl.AutoSize = true;
            this.labelTrasl.Location = new System.Drawing.Point(12, 407);
            this.labelTrasl.Name = "labelTrasl";
            this.labelTrasl.Size = new System.Drawing.Size(107, 18);
            this.labelTrasl.TabIndex = 14;
            this.labelTrasl.Text = "Translation";
            // 
            // labelRot
            // 
            this.labelRot.AutoSize = true;
            this.labelRot.Location = new System.Drawing.Point(12, 477);
            this.labelRot.Name = "labelRot";
            this.labelRot.Size = new System.Drawing.Size(80, 18);
            this.labelRot.TabIndex = 15;
            this.labelRot.Text = "Rotation";
            this.labelRot.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(9, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "Nodo selecionado: Ninguno";
            // 
            // resetScale
            // 
            this.resetScale.Location = new System.Drawing.Point(123, 329);
            this.resetScale.Name = "resetScale";
            this.resetScale.Size = new System.Drawing.Size(75, 23);
            this.resetScale.TabIndex = 17;
            this.resetScale.Text = "Reset";
            this.resetScale.UseVisualStyleBackColor = true;
            this.resetScale.Click += new System.EventHandler(this.resetScale_Click);
            // 
            // resetTransl
            // 
            this.resetTransl.Location = new System.Drawing.Point(123, 399);
            this.resetTransl.Name = "resetTransl";
            this.resetTransl.Size = new System.Drawing.Size(75, 23);
            this.resetTransl.TabIndex = 18;
            this.resetTransl.Text = "Reset";
            this.resetTransl.UseVisualStyleBackColor = true;
            this.resetTransl.Click += new System.EventHandler(this.resetTransl_Click);
            // 
            // resetRot
            // 
            this.resetRot.Location = new System.Drawing.Point(123, 472);
            this.resetRot.Name = "resetRot";
            this.resetRot.Size = new System.Drawing.Size(75, 23);
            this.resetRot.TabIndex = 19;
            this.resetRot.Text = "Reset";
            this.resetRot.UseVisualStyleBackColor = true;
            this.resetRot.Click += new System.EventHandler(this.resetRot_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1005, 604);
            this.Controls.Add(this.resetRot);
            this.Controls.Add(this.resetTransl);
            this.Controls.Add(this.resetScale);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelRot);
            this.Controls.Add(this.labelTrasl);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.rotZ);
            this.Controls.Add(this.scaleY);
            this.Controls.Add(this.scaleZ);
            this.Controls.Add(this.translX);
            this.Controls.Add(this.translY);
            this.Controls.Add(this.translZ);
            this.Controls.Add(this.rotX);
            this.Controls.Add(this.rotY);
            this.Controls.Add(this.scaleX);
            this.Controls.Add(this.sceneTree);
            this.Controls.Add(this.save);
            this.Controls.Add(this.open);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.scaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.translZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.translY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.translX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        private void resetScale_Click(object sender, EventArgs e)
        {
            scaleX.Value = 1.0M;
            scaleY.Value = 1.0M;
            scaleZ.Value = 1.0M;
        }

        private void resetTransl_Click(object sender, EventArgs e)
        {
            translX.Value = 0.0M;
            translY.Value = 0.0M;
            translZ.Value = 0.0M;
        }

        private void resetRot_Click(object sender, EventArgs e)
        {
            rotX.Value = 0.0M;
            rotY.Value = 0.0M;
            rotZ.Value = 0.0M;

        }

    }

}

