// ReSharper disable InconsistentNaming
// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable InvertIf

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // 000000 000000	NOP —	    No Operation            
    // 000000 000010	OPTION —	Load OPTION register
    // 000000 000011	SLEEP —	    Go into Standby mode
    // 000000 000100	CLRWDT	    Clear Watchdog Timer
    // 000000 000fff	TRIS f	    Load TRIS register
    // 000000 1fffff	MOVWF f	    Move W to f
    // 
    // 000001 000000	CLRW —	    Clear W
    // 000001 1fffff	CLRF f	    Clear f
    // 
    // 000010 dfffff	SUBWF f,d	Subtract W from f
    // 000011 dfffff	DECF f,d	Decrement f
    // 000100 dfffff	IORWF f,d	Inclusive OR W with f
    // 000101 dfffff	ANDWF f,d	AND W with f
    // 000110 dfffff	XORWF f,d	Exclusive OR W with f
    // 000111 dfffff	ADDWF f,d	Add W and f
    // 001000 dfffff	MOVF f,d	Move f
    // 001001 dfffff	COMF f,d	Complement f
    // 001010 dfffff	INCF f,d	Increment f
    // 001011 dfffff	DECFSZ f,d	Decrement f, Skip if 0
    // 001100 dfffff	RRF f,d	    Rotate right f through Carry
    // 001101 dfffff	RLF f,d	    Rotate left f through Carry
    // 001110 dfffff	SWAPF f,d	Swap f
    // 001111 dfffff	INCFSZ f,d	Increment f, Skip if 0
    // 
    // 0100bb bfffff	BCF f, b	Bit Clear f
    // 0101bb bfffff	BSF f, b	Bit Set f
    // 0110bb bfffff	BTFSC f, b	Bit Test f, Skip if Clear
    // 0111bb bfffff	BTFSS f, b	Bit Test f, Skip if Set
    // 1000kk kkkkkk	RETLW k	    Return, place Literal in W
    // 1001kk kkkkkk	CALL k	    Call Subroutine
    // 
    // 101kkk kkkkkk	GOTO k	    Unconditional branch
    // 1100kk kkkkkk	MOVLW k	    Move literal to W
    // 1101kk kkkkkk	IORLW k	    Inclusive OR literal with W
    // 1110kk kkkkkk	ANDLW k	    AND literal with W
    // 1111kk kkkkkk	XORLW k	    Exclusive OR literal to W
    
    public class InstructionDecoder
    {
        private readonly Instruction[] _instructions;
        private readonly Instruction _nop;
        private readonly Instruction _option;
        // private readonly Instruction _sleep;
        private readonly Instruction _tris;
        // private readonly Instruction _clrwdt;
        private readonly Instruction _clrw;
        private readonly Instruction _clrf;
        private readonly Instruction _movwf;

        public InstructionDecoder(Microcontroller microcontroller)
        {
            var allInstructions = microcontroller.AllInstructions;
            _nop = allInstructions["NOP"];
            _option = allInstructions["OPTION"];
            // _sleep = allInstructions["SLEEP"];
            _tris = allInstructions["TRIS"];
            // _clrwdt = allInstructions["CLRWDT"];
            _clrw = allInstructions["CLRW"];
            _clrf = allInstructions["CLRF"];
            _movwf = allInstructions["MOVWF"];
            
            _instructions = new []
            {
                /* 00 00 00 */ allInstructions["NOP"],    // allInstructions["NOP"],
                /* 00 00 01 */ allInstructions["NOP"],    // allInstructions["NOP"],
                /* 00 00 10 */ allInstructions["SUBWF"],
                /* 00 00 11 */ allInstructions["NOP"],    // allInstructions["DECF"],
                /* 00 01 00 */ allInstructions["IORWF"],
                /* 00 01 01 */ allInstructions["ANDWF"],
                /* 00 01 10 */ allInstructions["XORWF"],
                /* 00 01 11 */ allInstructions["ADDWF"],
                /* 00 10 00 */ allInstructions["NOP"],    // allInstructions["MOVF"],
                /* 00 10 01 */ allInstructions["COMF"],
                /* 00 10 10 */ allInstructions["NOP"],    // allInstructions["INCF"],
                /* 00 10 11 */ allInstructions["NOP"],    // allInstructions["DECFSZ"],
                /* 00 11 00 */ allInstructions["NOP"],    // allInstructions["RRF"],
                /* 00 11 01 */ allInstructions["NOP"],    // allInstructions["RLF"],
                /* 00 11 10 */ allInstructions["NOP"],    // allInstructions["SWAPF"],
                /* 00 11 11 */ allInstructions["NOP"],    // allInstructions["INCFSZ"],
                /* 01 00 00 */ allInstructions["BCF"],
                /* 01 00 01 */ allInstructions["BCF"],
                /* 01 00 10 */ allInstructions["BCF"],
                /* 01 00 11 */ allInstructions["BCF"],
                /* 01 01 00 */ allInstructions["BSF"],
                /* 01 01 01 */ allInstructions["BSF"],
                /* 01 01 10 */ allInstructions["BSF"],
                /* 01 01 11 */ allInstructions["BSF"],
                /* 01 10 00 */ allInstructions["NOP"],    // allInstructions["BTFSC"],
                /* 01 10 00 */ allInstructions["NOP"],    // allInstructions["BTFSC"],
                /* 01 10 00 */ allInstructions["NOP"],    // allInstructions["BTFSC"],
                /* 01 10 00 */ allInstructions["NOP"],    // allInstructions["BTFSC"],
                /* 01 11 00 */ allInstructions["NOP"],    // allInstructions["BTFSS"],
                /* 01 11 00 */ allInstructions["NOP"],    // allInstructions["BTFSS"],
                /* 01 11 00 */ allInstructions["NOP"],    // allInstructions["BTFSS"],
                /* 01 11 00 */ allInstructions["NOP"],    // allInstructions["BTFSS"],
                /* 10 00 00 */ allInstructions["NOP"],    // allInstructions["RETLW"],
                /* 10 00 00 */ allInstructions["NOP"],    // allInstructions["RETLW"],
                /* 10 00 00 */ allInstructions["NOP"],    // allInstructions["RETLW"],
                /* 10 00 00 */ allInstructions["NOP"],    // allInstructions["RETLW"],
                /* 10 01 00 */ allInstructions["NOP"],    // allInstructions["CALL"],
                /* 10 01 00 */ allInstructions["NOP"],    // allInstructions["CALL"],
                /* 10 01 00 */ allInstructions["NOP"],    // allInstructions["CALL"],
                /* 10 01 00 */ allInstructions["NOP"],    // allInstructions["CALL"],
                /* 10 10 00 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 10 01 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 10 10 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 10 11 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 11 00 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 11 01 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 11 10 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 10 11 11 */ allInstructions["NOP"],    // allInstructions["GOTO"],
                /* 11 00 00 */ allInstructions["NOP"],    // allInstructions["MOVLW"],
                /* 11 00 01 */ allInstructions["NOP"],    // allInstructions["MOVLW"],
                /* 11 00 10 */ allInstructions["NOP"],    // allInstructions["MOVLW"],
                /* 11 00 11 */ allInstructions["NOP"],    // allInstructions["MOVLW"],
                /* 11 01 00 */ allInstructions["IORLW"],
                /* 11 01 01 */ allInstructions["IORLW"],
                /* 11 01 10 */ allInstructions["IORLW"],
                /* 11 01 11 */ allInstructions["IORLW"],
                /* 11 10 00 */ allInstructions["ANDLW"],
                /* 11 10 01 */ allInstructions["ANDLW"],
                /* 11 10 10 */ allInstructions["ANDLW"],
                /* 11 10 11 */ allInstructions["ANDLW"],
                /* 11 11 00 */ allInstructions["XORLW"],
                /* 11 11 01 */ allInstructions["XORLW"],
                /* 11 11 10 */ allInstructions["XORLW"],
                /* 11 11 11 */ allInstructions["XORLW"],
            };
        }

        public Instruction Decode(int instructionCode)
        {
            var opcode = (instructionCode & 0b_111111_000000) >> 6;
            if (opcode >= 0b_000010)
                return _instructions[opcode];

            if (opcode == 0)
            {
                var subCode = instructionCode & 0b_111111; 
                if (subCode <= 0b_000111) 
                    return
                        subCode == 0b_000000 ? _nop :
                        subCode == 0b_000010 ? _option :
            //             subCode == 0b_000011 ? _sleep :
            //             subCode == 0b_000100 ? _clrwdt :
                        _tris;
                return (subCode & 0b_100000) > 0 ? _movwf : _nop;
            }

            if (opcode == 1)
            {
                var subCode = instructionCode & 0b_111111;
                return subCode == 0 ? _clrw : 
                    subCode >= 0b_100000 ? _clrf :
                    _nop;
            }

            return _nop;
        }
    }
}