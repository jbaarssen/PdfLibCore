#ifndef PDFLIBCORE_H
#define PDFLIBCORE_H

__declspec(dllexport) typedef unsigned int FPDF_ERR;
void fpdf_init_err(FPDF_ERR err) = delete;

__declspec(dllexport) typedef unsigned long FPDF_COLOR;
void fpdf_init_color(FPDF_COLOR err) = delete;

#endif // PDFLIBCORE_H
