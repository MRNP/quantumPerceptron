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

    operation Oracle(register: Qubit[]) : ()
    {
        body{
            ApplyToEach(H,register);
            //S(first);
            //S(second);

            H(register[1]);
            CNOT(register[0], register[1]);
            H(register[1]);

            //S(first);
            //S(second);
            ApplyToEach(H,register);
        }
    }

    

    operation PerceptronOracle(register: Qubit[]) : ()
    {
        body{
            ApplyToEach(H,register);
            //S(first);
            //S(second);

            H(register[1]);
            CNOT(register[0], register[1]);
            H(register[1]);

            //S(first);
            //S(second);
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

    operation Perceptron() : (Int, Int)
    {
        body{
            mutable cregister = new Int[2];

            using(register = Qubit[2]){
                Set(Zero, register[0]);
                Set(Zero, register[1]);

                //Cambio fasi con oracolo
                Oracle(register);

                //uso grover per fare ricerca
                Grover(register);
                
                if(M(register[0]) == One) {set cregister[0] = 1;};
                if(M(register[1]) == One) {set cregister[1] = 1;};
                // Reset all of the qubits that we used before releasing them
                ResetAll(register);
            }

            return (cregister[0], cregister[1]);
        }
    }
}
