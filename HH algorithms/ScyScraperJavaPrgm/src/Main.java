import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Map.Entry;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class Main
{
	 
	
	 public static void main(String args[]) 
	    {
		    MyEx mEx = new MyEx();		
		    // ArrayList<Integer> clues = new ArrayList<Integer> (Arrays.asList(0,0,0,2,2,0,0,0,0,6,3,0,0,4,0,0,0,0,4,4,0,3,0,0));
		    ArrayList<Integer> clues = new ArrayList<Integer>();
		    BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
		    String lim = null;
		    try {
		        lim = reader.readLine();
		    } catch (IOException e) {
		        e.printStackTrace();
		    }
		    

		    for (String field : lim.split(",+")) {
		    	Integer numb = Integer.parseInt(field);
		    	clues.add(numb);
		    }
		        		 	
	    	ArrayList<ArrayList<Integer>> rez = new ArrayList<ArrayList<Integer>>();
	    	rez.addAll(mEx.solvePuzzle(clues));
	    	
	    	for(int i = 0; i< rez.size();i++)
	    	{
	    		for(int j = 0; j < rez.get(i).size();j++)
	    		{
	    			System.out.print(rez.get(i).get(j));
	    			if( !(i == rez.size() - 1 && j == rez.get(i).size()-1) ) { System.out.print(',');}	 
	    		}
	    	}
	    	
	    }
	   
}
 class MyEx 
 {
	    	
	    	int N = 6;
	        int SIDES = 4;
	        private  int MASK = (1 << N) - 1;
	        int[] possible = new int[N * N];
	        int[] s = new int [SIDES * N];
	        int[] e = new int[SIDES * N]; 
	        int[] inc = new int[SIDES*N];
	        int[][] results = new int [N][N];

	        ArrayList<Integer> my_clues;
	        ArrayList<Integer> order = new ArrayList<Integer>() ;

	    	
       void set_value(int x, int v)
       {
           int m = MASK ^ (1 << v);
           int s_row = x - x % N;
           int s_col = x % N;
           for (int i = 0; i < N; i++)
           {
               possible[s_row + i] &= m;
               possible[s_col + i * N] &= m;
           }
           possible[x] = 1 << v;
       }
       int check_unique()
       {
           int n_decides = 0;
           for (int i = 0; i < SIDES / 2 * N; i++)
           {

               Map<Integer, ArrayList<Integer>> possible_indices = new HashMap<Integer, ArrayList<Integer>>();
               for (int j = s[i], k = 0; k < N; j += inc[i], k++)
               {
                   for (int l = 0; l < N; l++)
                       if (((1 << l) & possible[j])!= 0) 
                       {
                           if (!possible_indices.containsKey(l))
                           {
                               possible_indices.put(l, new ArrayList<Integer>());
                           }

                           possible_indices.get(l).add(j); 
                       }
               }
               for (Map.Entry<Integer, ArrayList<Integer>> iter : possible_indices.entrySet() )
               {
            	  int val = iter.getKey();
            	
                   if (iter.getValue().size()==1)
                   {
                	   ArrayList<Integer> values = iter.getValue();
                	   int idx = values.get(0);
                       //  int idx = iter.Value[0]; //Áûכמ iter.Value[0];
                       
                	   if (possible[idx] != (1 << val))
                       {
                           n_decides++;
                           set_value(idx, val);
                       }
                   }
               }
                             
           }
           return n_decides;
       }
       public int count_possible(int val)
       {
           int n = 0;
           while (val != 0) //TODO:
           {
               n += val & 1;
               val >>= 1;
           }
           return n;
       }
       public boolean valid()
       {
           for (int i = 0; i < SIDES * N; i++)
           {
               if (my_clues.get(i) == 0) continue;

               boolean is_decided = true;
               for (int j = s[i], k = 0; k < N; j += inc[i], k++)
               {
                   if (count_possible(possible[j]) != 1)
                   {
                       is_decided = false;
                       break;
                   }
               }

               if (is_decided)
               {
                   int largest = 0, n_clue = 0;
                   for (int j = s[i], k = 0; k < N; j += inc[i], k++)
                   {
                       if (largest < possible[j])
                       {
                           n_clue++;
                           largest = possible[j];
                       }
                   }
                   if (n_clue != my_clues.get(i)) return false;
               }
           }

           return true;
       }
       public void write_results()
       {
           for (int i = 0; i < N * N; i++)
           {
               int x = i / N, y = i % N;
               for (int j = 0; j < N; j++)
               {
                   if ((1 << j) == possible[i])
                   {
                       results[x][y] = j + 1;
                       break;
                   }
               }
           }
       }
       public boolean dfs(int idx)
       {
            if (idx >= order.size()) 
           {
               if (valid())
               {
                   write_results();
                   return true;
               }
               return false;
           }

           int i = order.get(idx);
           int[] possible_bak = new int[N * N];

           System.arraycopy(possible,0, possible_bak,0, N * N);
           
           for (int j = 0; j < N; j++)
           {
               int m = (1 << j) & possible[i];
               if (m == 0) continue;

               set_value(i, j);
               boolean found = false;
               if (valid())
               {
                   found = dfs(idx + 1);
               }
               if (found)
               {
                   return true;
               }
               System.arraycopy(possible_bak,0, possible,0,  N * N); 
              
           }
           return false;
       }
       public ArrayList<ArrayList<Integer>> solvePuzzle(ArrayList<Integer> clues)
       {
           my_clues = clues;
           for (int i = 0; i < N * N; i++) possible[i] = MASK;

           for (int i = 0; i < N; i++)
           {
               s[i] = i;
               e[i] = (N - 1) * N + i;
               inc[i] = N;
           }

           for (int i = 0, j = N; i < N; i++, j++)
           {
               s[j] = i * N + N - 1;
               e[j] = i * N;
               inc[j] = -1;
           }

           for (int i = 0, j = 2 * N; i < N; i++, j++)
           {
               s[j] = N * N - 1 - i;
               e[j] = N - 1 - i;
               inc[j] = -N;
           }

           for (int i = 0, j = 3 * N; i < N; i++, j++)
           {
               s[j] = (N - i - 1) * N;
               e[j] = (N - i) * N - 1;
               inc[j] = 1;
           }

           for (int i = 0; i < SIDES * N; i++)
           {
               if (my_clues.get(i) == 0) continue;
               for (int j = s[i], k = 0; k < N; j += inc[i], k++)
               {
                   int m = MASK;
                   for (int l = N + k - my_clues.get(i) + 1; l < N; l++) m ^= 1 << l;
                   possible[j] &= m;
               }
           }

           while (check_unique() > 0) ;

         
           ArrayList<ArrayList<Integer>> DictSortVal = new ArrayList<ArrayList<Integer>>();
       	   ArrayList<ArrayList<Integer>>  DictSortKeyVal = new ArrayList<ArrayList<Integer>> ();
       	   ArrayList<Integer> Key = new ArrayList<Integer> ();
       	   ArrayList<Integer> Val = new ArrayList<Integer> () ;
           
       	   for (int i = 0; i < N * N; i++)
       	   {
       		int n_possible = count_possible(possible[i]);
           	if (n_possible > 1)
           	{
        	   Key.add(n_possible);
        	   Val.add(i) ;    
           	}
       	   }
       	  DictSortVal = BubbleSortVal( Key, Val);
          DictSortKeyVal = BubbleSortKey(DictSortVal.get(0), DictSortVal.get(1)); 
           
           order.clear();
           for (int i = 0; i < DictSortKeyVal.get(1).size(); i++)
           {
               order.add(DictSortKeyVal.get(1).get(i)); 
           }
           dfs(0);

           ArrayList<ArrayList<Integer>> r = new ArrayList<ArrayList<Integer>>();   
           for (int i = 0; i < N; i++)
           {
               ArrayList<Integer> vec = new ArrayList<Integer>();
               {
                   for (int j = 0; j < N; j++) 
                   {
                	   vec.add(results[i][j]);
                   }
                   r.add(vec);
               }
               
           }
           return r;
       }
       

   	public void Swap(ArrayList<Integer> arrList, int ind1, int ind2)
       {
   		int tmp = arrList.get(ind1);
   		arrList.set(ind1, arrList.get(ind2));
   	    arrList.set(ind2, tmp);
       }
   	public ArrayList<ArrayList<Integer>> BubbleSortKey( ArrayList<Integer> key, ArrayList<Integer> val)
       {
           int len = key.size();
           for (int i = 1; i < len; i++)
           {
               for (int j = 0; j < len - i; j++)
               {
                   if (key.get(j) > key.get(j+1))
                   {
                       Swap( key, j, j + 1);
                       Swap( val, j, j + 1 );
                   }
               }
           }
           ArrayList<Integer> sortKey = new ArrayList<Integer>();
           ArrayList<Integer> sortVal = new ArrayList<Integer>();

           for (Integer e : key) {sortKey.add(e);}
           for (Integer e : val) {sortVal.add(e);}
           
           ArrayList<ArrayList<Integer>>  custDict = new ArrayList<ArrayList<Integer>> ();
           custDict.add(sortKey);
           custDict.add(sortVal);
           return custDict;
       }
   	public ArrayList<ArrayList<Integer>> BubbleSortVal(ArrayList<Integer> key, ArrayList<Integer> val)
       {
   		int len = key.size();
           for (int i = 1; i < len; i++)
           {
               for (int j = 0; j < len - i; j++)
               {
                   if (val.get(j) > val.get(j+1))
                   {
                       Swap( key, j, j + 1);
                       Swap( val, j, j + 1 );
                   }
               }
           }

           ArrayList<Integer> sortKey = new ArrayList<Integer>();
           ArrayList<Integer> sortVal = new ArrayList<Integer>();

           for (Integer e : key) {sortKey.add(e);}
           for (Integer e : val) {sortVal.add(e);}
           
           ArrayList<ArrayList<Integer>>  custDict = new ArrayList<ArrayList<Integer>> ();
           custDict.add(sortKey);
           custDict.add(sortVal);
     
           return custDict;
       }	
   	
   }


