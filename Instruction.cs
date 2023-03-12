public class Instruction
{
    public string TableName { get; set; }
    public bool NeedCreateTable { get; set; }
    public List<Column> Columns { get; set; }
}
