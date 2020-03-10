import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
//import java.util.Arrays;
import java.util.Collections;


public class Main
{
	public static void main(String[] args)
	{
		ArrayList<ArrayList<Character>> ListCode = new ArrayList<ArrayList<Character>>();
		CodeListGenerator clg = new CodeListGenerator();
	    MyEx mx = new MyEx();
	    
	    BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
	   // System.out.print("Please enter code : ");
	    String code = null;
	    try {
	        code = reader.readLine();
	    } catch (IOException e) {
	        e.printStackTrace();
	    }
	    ListCode = clg.GetListCode(code);
	    
	    mx.GetAllVariant(ListCode);
	}
	
}

class MyEx 
{
	private int[] IndexOfArrList;
	int[][] CodePanel = {
			
			new int[]{3,6,9},
            new int[]{2,5,8,0},
            new int[]{1,4,7}
        };


public void GetAllVariant(ArrayList<ArrayList<Character>> ArrList)
{
    IndexOfArrList = new int[ArrList.size()];
    for (int i = 0; i < IndexOfArrList.length; i++)
    {
        IndexOfArrList[i] = 0;
    }
    StringBuilder sb = new StringBuilder(); 
    
    boolean indexChange = false;
    
    for (int i = 0; i < IndexOfArrList.length; i++)
    {
        sb.append(ArrList.get(i).get(IndexOfArrList[i]));
        
        if (i == ArrList.size() - 1 )  
        {
        	sb.append(',');
           // sb.append(System.getProperty("line.separator"));
        	
            for (int j = IndexOfArrList.length - 1; j >= 0; j--)
            {
                if (IndexOfArrList[j] < ArrList.get(j).size())
                {
                    IndexOfArrList[j] += 1;
                    indexChange = true;
                }
              
                if (IndexOfArrList[j] == ArrList.get(j).size())
                {
                    if (IndexOfArrList[0] == ArrList.get(0).size())
                    {
                        indexChange = false; break;
                    }
                    IndexOfArrList[j] = 0 ;
                    indexChange = false;
                }
               
                if (indexChange){ indexChange = false; break;}
            }
            if (IndexOfArrList[0] > ArrList.get(0).size()-1) 
            {
                 break;
            }
            i = -1;
        }
    }
    sb.setLength(sb.length() - 1);
    String answer = sb.toString(); 
    System.out.println(answer);
}

}

class CodeListGenerator
{
    public int[][] CodePanel =
    {
        new int[]{3,6,9},
        new int[]{2,5,8,0},
        new int[]{1,4,7}
    };

    public ArrayList<ArrayList<Character>> GetListCode(String code)
    {
    	ArrayList<ArrayList<Character>> ListCode = new ArrayList<ArrayList<Character>>();
        
        char[] codeArr = GetCodeAsCharArray(code);

        for (int i = 0; i < codeArr.length; i++)
        {
            ListCode.add(new ArrayList<Character>());
            ListCode.get(i).add(codeArr[i]);// а, ну мы собственно записываем сам символ
            SetCodeVariants((int)Character.getNumericValue(codeArr[i]), i, ListCode);
            Collections.sort(ListCode.get(i));
            
        }
        return ListCode;
    }

    public void SetCodeVariants(int symbol, int symbolCurrentIndex, ArrayList<ArrayList<Character>> ListCode)
    {
        int [] indexArr = new int[2];
        indexArr = GetIndexCodePanelElement(symbol, indexArr); //y,x
        GetCodeWithNewLeftElement(indexArr, symbolCurrentIndex, ListCode);
        GetCodeWithNewUpElement(indexArr, symbolCurrentIndex, ListCode);
        GetCodeWithNewRightElement(indexArr, symbolCurrentIndex, ListCode);
        GetCodeWithNewDownElement(indexArr, symbolCurrentIndex, ListCode);
    }

    
    private void GetCodeWithNewLeftElement(int[] elementCpIndex, int elementOldIndex, ArrayList<ArrayList<Character>> ListCode)
    {
        if ((elementCpIndex[1] - 1) >= 0)
        {
            int leftElement = CodePanel[elementCpIndex[0]][elementCpIndex[1] - 1];
            ListCode.get(elementOldIndex).add((Integer.toString(leftElement).charAt(0)));
        }
    }
    private void GetCodeWithNewUpElement(int[] elementCpIndex, int elementOldIndex, ArrayList<ArrayList<Character>> ListCode)
    {
        if ((elementCpIndex[0] - 1) >= 0 &&
            CodePanel[elementCpIndex[0] - 1].length >= elementCpIndex[1]+1)
        {
            int upElement = CodePanel[elementCpIndex[0] - 1][elementCpIndex[1]];
            ListCode.get(elementOldIndex).add((Integer.toString(upElement).charAt(0)));
        }
    }
    private void GetCodeWithNewRightElement(int[] elementCpIndex, int elementOldIndex, ArrayList<ArrayList<Character>> ListCode)
    {
        if ((elementCpIndex[1] + 1) < CodePanel[elementCpIndex[0]].length)
        {
            int rightElement = CodePanel[elementCpIndex[0]][elementCpIndex[1] + 1];
            ListCode.get(elementOldIndex).add((Integer.toString(rightElement).charAt(0)));
        }
    }
    private void GetCodeWithNewDownElement(int[] elementCpIndex, int elementOldIndex, ArrayList<ArrayList<Character>> ListCode)
    {
        if ((elementCpIndex[0] + 1) < CodePanel.length &&
            (CodePanel[elementCpIndex[0] + 1].length >= elementCpIndex[1] + 1))
        {
            int downElement = CodePanel[elementCpIndex[0] + 1][elementCpIndex[1]];
            ListCode.get(elementOldIndex).add((Integer.toString(downElement).charAt(0)));
        }
    }


    //получить индекс элемента кода на СodePanel
    private int[] GetIndexCodePanelElement(int symbol,  int[] elementCpIndex)
    {
        int x = -1;
        int y = -1;
        
        for (int i = 0; i < CodePanel.length; i++)
        {
            for (int j = 0; j < CodePanel[i].length; j++)
            {
                if (symbol == CodePanel[i][j]) { y = i; x = j; break; }
            }
        }
        int [] valArr = {y,x};
        
        return valArr;
    }

    // преобразовать код в массив char
    public char[] GetCodeAsCharArray(String code)
    {
        return code. toCharArray();
    }
}
