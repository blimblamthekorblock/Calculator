using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public List<string> CsvQueue1 = new List<string>();
        public bool debugWriteConsole = true;

        private enum Operation
        {
            Nul,
            Add,
            Sub,
            Mul,
            Div,
        }

        private enum ItemType
        {
            Integrals,
            Operation,
        }

        private class CalcItem
        {
            public int ID; // item order in the line
            public ItemType Type; // number or operation
            public Operation Op; // add, substract, etc
            public char OpByte;
            public ulong Number; // value for numbers
        }

        private void Log(string input)
        {
            if (debugWriteConsole)
                Console.WriteLine(input);
            richTextBox_log.Text = $"{richTextBox_log.Text}\n{input}"; // just for logging

            CsvQueue1.Add(input); // used to dump logs to a text file
        }

        private void CsvWrite(List<string> lines, string filename)
        {
            using (var writer = new StreamWriter(filename))
                foreach (var line in lines)
                    writer.WriteLine(line);
        }

        private void button_clearTextBoxes(object sender, EventArgs e)
        {
            richTextBox_left.Text = "";
            richTextBox_right.Text = "";
            richTextBox_log.Text = "";
        }

        private void button_dumpLogs(object sender, EventArgs e)
        {
            CsvWrite(CsvQueue1, "output.log");
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            // always scroll the box to the bottom
            richTextBox_log.SelectionStart = richTextBox_log.Text.Length;
            richTextBox_log.ScrollToCaret();
        }

        public Form1()
        {
            InitializeComponent();
            richTextBox_log.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox_left.KeyPress += new KeyPressEventHandler(richTextBox1_KeyPress);
            richTextBox_right.KeyPress += new KeyPressEventHandler(richTextBox2_KeyPress);
        }

        void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ReadTextBox(e, richTextBox_left);
        }

        void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ReadTextBox(e, richTextBox_right);
        }

        private void ReadTextBox(KeyPressEventArgs e, RichTextBox richtextbox)
        {
            var c = e.KeyChar;

            // Do a simple check if the line contains valid characters, 0 - 9, a - f, A - F, + - * /
            if (!ValidCharacters.ValidChars_all.Contains((byte)c))
            {
                e.Handled = true;
                // Log($"ERROR: unsupported key: {e.KeyChar}");
            }

            var tempText_left = richTextBox_left.Text;
            var tempText_right = richTextBox_right.Text;

            if (e.KeyChar == 0x0D) // seems to be useless
            {
                e.Handled = true;

                richtextbox = ParseLines(richtextbox);
            }
        }

        private RichTextBox ParseLines(RichTextBox textfile)
        {
            // CsvAdd($"Attempting to parse: {HistPrevOperation}");
            var lines = textfile.Text.Split("\n".ToCharArray());

            // var i = -1;
            // foreach (var line in lines)
            // {
            //     i++;
            //     CsvAdd($"Line {i}: {line}");
            // }

            if (lines.Length == 0)
                return textfile;

            // lines contains every line in the text box. Only select the second to last. Last should be a new empty line.
            var lineItems = SplitLine(lines[lines.Length - 2]);

            if (lineItems.Count == 0)
                return textfile;

            // If the input is: 1000+2000*2 or 1000 + 2000 * 2
            // Then lineItems will contain:
            /*
             * 1000
             * +
             * 2000
             * *
             * 2
             * 
             * for 1000:
             * int ID        = 0
             * ItemType Type = ItemType.Number
             * Operation Op  = Operation.None
             * char OpByte   = 0x0
             * ulong Number  = 0x0000000000001000
             * 
             */

            foreach (var a in lineItems)
            {
                var output = $"Line Item: " +
                    $"ID {a.ID}, " +
                    $"Type {a.Type}, " +
                    $"OP {a.Op}, " +
                    $"Val {a.Number:X16}, " +
                    $"";

                Log(output);
            }
            // ParseLine(lines[lines.Length - 2]); 

            DoOperations(lineItems);

            return textfile;
        }

        private List<CalcItem> SplitLine(string line)
        {
            var chars = line.ToCharArray();

            foreach (var char_ in chars)
            {
                if (!ValidCharacters.ValidChars_all.Contains((byte)char_))
                {
                    // Log($"ERROR: line contains invalid characters: {char_}");
                    // Log($"ERROR: missing valid character: {char_} {(byte)char_:X2}");
                    return new List<CalcItem>();
                }
            }

            var items = new List<CalcItem>();
            var charsQueue = new List<char>();
            var items_temp = new List<List<char>>();

            var d = "";
            ulong convertedVal = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                // if it finds an operator
                if (ValidCharacters.ValidChars_ops.Contains((byte)chars[i]))
                {
                    // items_temp list = list of all items on a line, such as numbers, operators

                    // incase there was a number before this operator, the char queue would be filled. return it as a new item, then add the operator as another item
                    items_temp.Add(charsQueue);

                    d = "";
                    foreach (var a in charsQueue)
                        d = $"{d}{a}";
                    convertedVal = 0;
                    ulong.TryParse(d, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out convertedVal);
                    // Log($"Parsed: {d}: 0x{convertedVal:X16}, {convertedVal:X16}");
                    items.Add(new CalcItem
                    {
                        ID = items_temp.Count - 1,
                        Type = ItemType.Integrals,
                        Number = convertedVal,
                        Op = Operation.Nul,
                        OpByte = (char)0x0
                    });

                    // reset current chars list
                    charsQueue = new List<char>();

                    charsQueue.Add(chars[i]);
                    // add this operator to the items list
                    items_temp.Add(charsQueue);
                    // reset current chars list since operators should be only one char long
                    charsQueue = new List<char>();

                    var op = Operation.Nul;
                    switch ((byte)chars[i])
                    {
                        case 0x2A: op = Operation.Mul; break;
                        case 0x2B: op = Operation.Add; break;
                        case 0x2F: op = Operation.Div; break;
                        case 0x2D: op = Operation.Sub; break;
                    }

                    items.Add(new CalcItem
                    {
                        ID = items_temp.Count - 1,
                        Type = ItemType.Operation,
                        Op = op,
                        OpByte = chars[i],
                        Number = 0,
                    });

                    continue;
                }

                if (ValidCharacters.ValidChars_numbers.Contains((byte)chars[i]))
                {
                    // continue adding chars if it's a number
                    charsQueue.Add(chars[i]);
                    continue;
                }
            }

            // assuming no op was found, or there's a space, or end of line, add the current chars queue
            items_temp.Add(charsQueue);

            d = "";
            foreach (var a in charsQueue)
                d = $"{d}{a}";
            convertedVal = 0;
            ulong.TryParse(d, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out convertedVal);
            // Log($"Parsed: {d}: 0x{convertedVal:X16}, {convertedVal:X16}");
            items.Add(new CalcItem
            {
                ID = items_temp.Count - 1,
                Type = ItemType.Integrals,
                Number = convertedVal,
                Op = Operation.Nul,
                OpByte = (char)0x0
            });

            // reset current chars list
            charsQueue = new List<char>();

            var lineItems = new List<string>();

            // temp: draw current line items
            var k = -1;
            foreach (var a in items_temp)
            {
                k++;
                d = "";
                foreach (var b in a)
                    d = $"{d}{b}";

                lineItems.Add(d);
                // Log($"Line item {k:D2}: {d}");
            }

            return items;
        }

        private void DoOperations(List<CalcItem> lineItems)
        {
            // Hardcode to only detect addition between 2 numbers for now.
            ulong nr1 = 0;
            ulong nr2 = 0;
            var op1 = Operation.Add;

            if (lineItems.Count > 0)
                nr1 = lineItems[0].Number;

            // maybe it shouldn't be like this
            ulong result = nr1;

            if (lineItems.Count == 3)
            {
                nr1 = lineItems[0].Number;
                op1 = lineItems[1].Op;
                nr2 = lineItems[2].Number;

                if (op1 == Operation.Add)
                    result = nr1 + nr2;
            }

            newLine = result;

            UpdateTextBoxes(richTextBox_left, $"\n{newLine:D16}");
            UpdateTextBoxes(richTextBox_right, $"\n{newLine:X16}");
        }

        private ulong newLine = 0;

        private void UpdateTextBoxes(RichTextBox richTextBox, string additional)
        {
            richTextBox.Text = $"{richTextBox.Text}{additional}\n";
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
        }
    }
}