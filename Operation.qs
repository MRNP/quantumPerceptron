namespace progettoreti
{
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Primitive;

    operation Set (desired: Result, q1: Qubit) : ()
    {
        body
        {
            let current = M(q1);
            if (desired != current)
            {
                X(q1);
            }
        }
    }

    operation SuperExample() : (Int[], Int[], Int)
    {
        body{
            mutable cregister = new Int[2];
            mutable superregister = new Int[4];
            mutable ntests = 100;

            using(register = Qubit[2]){
            set cregister[0] = 0;
            set cregister[1] = 0;

                for(test in 1..ntests){
                    Set(Zero, register[0]);
                    Set(Zero, register[1]);

                    //Creo superposition
                    ApplyToEach(H,register);
                    
                    if(M(register[0]) == One) {set cregister[0] = cregister[0] + 1;};
                    if(M(register[1]) == One) {set cregister[1] = cregister[1] + 1;};
                    if(M(register[0]) == Zero && M(register[1]) == Zero) {set superregister[0] = superregister[0] + 1;};
                    if(M(register[0]) == Zero && M(register[1]) == One) {set superregister[1] = superregister[1] + 1;};
                    if(M(register[0]) == One && M(register[1]) == Zero) {set superregister[2] = superregister[2] + 1;};
                    if(M(register[0]) == One && M(register[1]) == One) {set superregister[3] = superregister[3] + 1;};

                    // Reset all of the qubits that we used before releasing them
                    ResetAll(register);
                }
            }

            return (cregister, superregister, ntests);
        }
    }

    operation Oracle(register: Qubit[]) : ()
    {
        body{
            ApplyToEach(H,register);
            //S(register[0]);
            ///S(register[1]);

            H(register[1]);
            CNOT(register[0], register[1]);
            H(register[1]);

            //S(register[0]);
            ///S(register[1]);
            ApplyToEach(H,register);
        }
    }

    

    operation PerceptronOracle(register: Qubit[], ρ1: Int, ρ2: Int) : ()
    {
        body{
            ApplyToEach(H,register);
            if(ρ1!=1){S(register[1]);}
            if(ρ2!=1){S(register[0]);}

            H(register[1]);
            CNOT(register[0], register[1]);
            H(register[1]);

            if(ρ1!=1){S(register[1]);}
            if(ρ2!=1){S(register[0]);}
            ApplyToEach(H,register);
        }
    }    

    operation Grover (register: Qubit[]) : ()
    {
        body
        {
            ApplyToEach(X,register);
            H(register[1]);
            CNOT(register[0], register[1]);
            H(register[1]);
            ApplyToEach(X,register);
            ApplyToEach(H,register);
        }
    }

    operation Perceptron(ρ1: Int, ρ2: Int) : (Int[], Int)
    {
        body{
            mutable cregister = new Int[2];
            mutable weights = new Bool[2];
            mutable ntests = 100;

            using(register = Qubit[2]){
            set cregister[0] = 0;
            set cregister[1] = 0;

                for(test in 1..ntests){
                    Set(Zero, register[0]);
                    Set(Zero, register[1]);

                    //Cambio fasi con oracolo
                    PerceptronOracle(register, ρ1, ρ2);

                    //uso grover per fare ricerca
                    Grover(register);
                    
                    if(M(register[0]) == One) {set cregister[0] = cregister[0] + 1;};
                    if(M(register[1]) == One) {set cregister[1] = cregister[1] + 1;};
                    // Reset all of the qubits that we used before releasing them
                    ResetAll(register);
                }
            }

            return (cregister, ntests);
        }
    }


    operation OneShotPerceptron() : (Int, Int)
    {
        body{
            mutable cregister = new Int[2];
            using(register = Qubit[2]){
                set cregister[0] = 0;
                set cregister[1] = 0;
                Set(Zero, register[0]);
                Set(Zero, register[1]);

                //Cambio fasi con oracolo
                Oracle(register);
                //PerceptronOracle(register, 1, 0);

                //uso grover per fare ricerca
                Grover(register);
                
                if(M(register[0]) == One) {set cregister[0] = cregister[0] + 1;};
                if(M(register[1]) == One) {set cregister[1] = cregister[1] + 1;};
                // Reset all of the qubits that we used before releasing them
                ResetAll(register);
            }

            return (cregister[0], cregister[1]);
        }
    }
}
