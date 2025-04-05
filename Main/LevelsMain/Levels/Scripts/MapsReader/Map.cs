[System.Serializable]

// �������������Ķ���
public class Note_data
{
    public int Type;
    public int ActiveTime;
    public int Position;
    public int Mining_Licky_WaitTime;
    public int Mining_HoldTime;
    public int Full_Length;
}

// ���Ƕ�����Ķ���
public class Map
{
    public string MapName;          // ��������
    public float Level;             // ����ȼ�
    public string Creator;          // ���ߣ�ָ�������ߣ�
    public string Collaboration;    // ���ߣ��������ߣ�
    public string Illustration;     // ���ߣ��������ߣ�          
    public int NotesCount;          // ��������
    public int BPM;                 // BPM
    public int Time;                // ����ʱ�����룩
    public Note_data[] Notes;       // ��������
}
