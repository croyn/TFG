using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public int numrows = 20;
    public int numcols = 20;

    private int lenght;
    private int shif;

    private Pattern chosenpattern;

    private int randommat;

    List<ParticleSystem.Particle> cloud;

    private bool hasbeencalledonce = false;

    // Start is called before the first frame update
    void Start()
    {
        ParticleType[] worldParticle = new ParticleType[4];
        worldParticle = WorldParticleAssingment.gettheworldparticles(ApplicationManager.instance.chosenWorld);
        for (int i = 0; i < worldParticle.Length; i++)
        {
            ParticleSystem.MainModule main = transform.GetChild(i+2).GetComponent<ParticleSystem>().main;
            Color c = worldParticle[i].GetColor();
            c.a = 0.5f;
            main.startColor = c;

            if (numcols > 0 && numrows > 0)
            {
                main.maxParticles = numcols * numrows;
            }
        }

        

        SetParticlePosition();

    }

    void OnEnable() {
        if (!hasbeencalledonce)
        {
            lenght = Random.Range(5, 15);
            shif = Random.Range(5, 15);
            SetPattern();
            hasbeencalledonce = true;
        }
        SetParticlePosition();
    }


    private void SetPattern() {
        Pattern[] patterns = (Pattern[])System.Enum.GetValues(typeof(Pattern));//new Pattern[] { Pattern.grid5by5innerbox, Pattern.grid5by5vertexandcenter, Pattern.grid5by5Xdiamond, Pattern.grid5by5Ydiamond, Pattern.grid5by5gridangle1, Pattern.grid5by5gridangle2 };
        for (int i = 0; i < patterns.Length; i++)
        {
            int rnd = Random.Range(0, patterns.Length);
            Pattern temp = patterns[rnd];
            patterns[rnd] = patterns[i];
            patterns[i] = temp;
        }
        chosenpattern = patterns[Random.Range(0, patterns.Length)];
        randommat = Random.Range(0, 4);
    }
    
    void SetParticlePosition() {
        if (numrows >= 1 && numcols >= 1)
        {
            
            cloud = new List<ParticleSystem.Particle>();
                float spacesize_X = 100.0f / numcols;
                float spacesize_Y = 100.0f / numrows;
                int cur_col = 0, cur_row = 0;
                for (int i = 0; i < numcols * numrows; i++)
                {
                    ParticleSystem.Particle p = new ParticleSystem.Particle();
                    p.position = new Vector3((cur_col * spacesize_X) - 50, 0, (cur_row * spacesize_Y) - 50);
                    Color c = WorldParticleAssingment.gettheworldparticles(ApplicationManager.instance.chosenWorld)[randommat].GetColor();
                    c.a = 0.5f;
                    p.startColor = c;
                    p.startSize = 0.5f;
                    applyPattern(chosenpattern, p, cur_col, cur_row);
                    
                    cur_col++;
                    if (cur_col > numcols)
                    {
                        cur_col = 0;
                        cur_row++;
                    }
                }
            
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i + 2).GetComponent<ParticleSystem>().Clear();
            }
            transform.GetChild(randommat + 2).GetComponent<ParticleSystem>().SetParticles(cloud.ToArray());
        }
    }

    void applyPattern(Pattern pattern, ParticleSystem.Particle particle, int curcol, int currow){
        
        switch (pattern) {
            case Pattern.grid3by3vertexandcenter:
                if (curcol % 2 == 0 && currow % 2 != 0) {
                    cloud.Add(particle);
                }
                if (curcol % 2 != 0 && currow % 2 == 0) {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid3by3diamond:
                if (curcol % 2 == 0 && currow % 2 == 0) {
                    cloud.Add(particle);
                }
                if (curcol % 2 != 0 && currow % 2 != 0) {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5vertexandcenter:
                curcol = curcol % 5;
                currow = currow % 5;
                if (curcol % 4 == 0 && currow % 4 == 0)
                {
                    cloud.Add(particle);
                }
                if (curcol  == 2 && currow  == 2)
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5Xdiamond:
                curcol = curcol % 5;
                currow = currow % 5;
                if (curcol == 2 && (currow  == 1 || currow == 3))
                {
                    cloud.Add(particle);
                }
                if (curcol % 4 == 0 && currow == 2)
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5Ydiamond:
                curcol = curcol % 5;
                currow = currow % 5;
                if (curcol == 2 && currow % 4 == 0)
                {
                    cloud.Add(particle);
                }
                if ((curcol == 1 || curcol == 3) && currow == 2)
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5innerbox:
                curcol = curcol % 5;
                currow = currow % 5;
                if ((curcol % 4 == 1 && (currow % 4 == 1 || currow % 4 == 3)) || (curcol % 4 == 3 && (currow % 4 == 1 || currow % 4 == 3)))
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5gridangle1:
                curcol = curcol % 5;
                currow = currow % 5;
                if ((curcol == 4 && currow == 1) || (curcol == 1 && currow == 0) || (curcol == 3 && currow == 4) || (curcol == 0 && currow == 3))
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.grid5by5gridangle2:
                curcol = curcol % 5;
                currow = currow % 5;
                if ((curcol == 4 && currow == 3) || (curcol == 1 && currow == 4) || (curcol == 3 && currow == 0) || (curcol == 0 && currow == 1))
                {
                    cloud.Add(particle);
                }
                break;
            case Pattern.shiftMake:
                if ((curcol + currow * numcols) % (lenght + shif) <= lenght)
                {
                    cloud.Add(particle);
                }
                break;
        }
    }
}

public enum Pattern {
    grid3by3vertexandcenter, grid3by3diamond, grid5by5vertexandcenter, grid5by5Xdiamond, grid5by5Ydiamond, grid5by5innerbox, grid5by5gridangle1, grid5by5gridangle2, shiftMake
}