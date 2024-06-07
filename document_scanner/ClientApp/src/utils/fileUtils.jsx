// Functie voor het valideren van bestandsformaten
export const validateFileType = (file, allowedTypes) => {
    return allowedTypes.includes(file.type);
};