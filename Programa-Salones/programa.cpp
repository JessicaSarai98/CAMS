#include <iostream>
#include <string>
#include <iomanip>
#include <sstream>
#include <vector>
#include <fstream>

using namespace std;

int main(){
    string id, nombre, fila, columna; //variables
    vector<string>F; //para guardar los datos leidos del folio
    vector<string>nom; //del nombre
    vector<string>sa; //del salon

   /* string file;
    cout <<"Ingresa el nombre del archivo a leer: ";
    cin >> file;

    int i=0; //num de lineas

    ifstream alum(file); //abriendo el archivo
    if(alum.is_open()){
        //ignora la primera linea
        string line=""; 
        getline(alum,line);

        while(!alum.eof()){
            getline(alum,folio,',');
            //F.push_back(stof(folio));
            getline(alum,nombre,',');
            //es.push_back(stof(esc));
            getline(alum,salon,'\n');
            //sa.push_back(stof(salon));
            i+=1; 
        }
        alum.close();
        cout <<"Numero de filas: "<<i-1<<endl;*/
        string line=""; 

        ifstream infile("Salones.csv");
        while(getline(infile,line)){
            stringstream strstr(line);
            string word="";
            while(getline(strstr,word,'\n')){
                F.push_back(word);
            }
        }
        for(int k=0;k<F.size();k++){
            cout<<F.at(k)<<"\n";
        }

   // }
    //else cout <<"No se encontró el archivo";
    system("pause");


    cout <<"Folio"<<"\t"<<"Nombre"<<"\t"<<"Salon"<<endl;
    string fo,no,so;
    for(int j=0;j<i;j++){
        cout<<F[j]<<"\t"<<nom[j]<<"\t"<<sa[j]<<endl;
        
            no=nom[j];
            so=sa[j];
        
        
    }
    
}
